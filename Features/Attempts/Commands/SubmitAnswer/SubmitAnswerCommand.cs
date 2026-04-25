/* File Overview
 * File: SubmitAnswerCommand.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Errors;
using ExaminationSystem.Features.Attempts.Common;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.Commands.SubmitAnswer;

public record SubmitAnswerCommand(Guid AttemptId, Guid StudentId, Guid QuestionId, Guid SelectedOptionId) : IRequest<Result>;

public class SubmitAnswerCommandHandler(ApplicationDbContext context, IAttemptLifecycleService lifecycleService)
    : IRequestHandler<SubmitAnswerCommand, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IAttemptLifecycleService _lifecycleService = lifecycleService;

    public async Task<Result> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
    {
        var attempt = await _context.QuizAttempts
            .FirstOrDefaultAsync(x => x.Id == request.AttemptId, cancellationToken);

        if (attempt is null)
        {
            return Result.Failure(AttemptError.AttemptNotFound(request.AttemptId));
        }

        if (attempt.StudentId != request.StudentId)
        {
            return Result.Failure(AttemptError.AccessDenied);
        }

        if (attempt.Status is QuizAttemptStatus.Submitted or QuizAttemptStatus.TimedOut)
        {
            return Result.Failure(AttemptError.AlreadySubmitted);
        }

        if (_lifecycleService.IsExpired(attempt))
        {
            await _lifecycleService.FinalizeAttemptAsync(attempt, timedOut: true, cancellationToken);
            return Result.Failure(AttemptError.AttemptExpired);
        }

        var question = await _context.Questions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.QuestionId && x.QuizId == attempt.QuizId, cancellationToken);

        if (question is null)
        {
            return Result.Failure(AttemptError.QuestionNotInQuiz);
        }

        var optionValid = await _context.Options
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.SelectedOptionId && x.QuestionId == request.QuestionId, cancellationToken);

        if (!optionValid)
        {
            return Result.Failure(AttemptError.OptionInvalid);
        }

        var existingAnswer = await _context.AttemptAnswers
            .FirstOrDefaultAsync(x => x.QuizAttemptId == request.AttemptId && x.QuestionId == request.QuestionId, cancellationToken);

        if (existingAnswer is null)
        {
            await _context.AttemptAnswers.AddAsync(new AttemptAnswer
            {
                QuizAttemptId = request.AttemptId,
                QuestionId = request.QuestionId,
                SelectedOptionId = request.SelectedOptionId,
                AnsweredAt = DateTime.UtcNow
            }, cancellationToken);
        }
        else
        {
            existingAnswer.SelectedOptionId = request.SelectedOptionId;
            existingAnswer.AnsweredAt = DateTime.UtcNow;
            existingAnswer.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

