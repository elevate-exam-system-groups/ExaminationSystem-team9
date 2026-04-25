/* File Overview
 * File: StartQuizCommand.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Attempts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Errors;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.Commands.StartQuiz;

public record StartQuizCommand(Guid QuizId, Guid StudentId) : IRequest<Result<StartQuizResponse>>;

public class StartQuizCommandHandler(ApplicationDbContext context)
    : IRequestHandler<StartQuizCommand, Result<StartQuizResponse>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<StartQuizResponse>> Handle(StartQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _context.Quizzes
            .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync(x => x.Id == request.QuizId, cancellationToken);

        if (quiz is null)
        {
            return Result.Failure<StartQuizResponse>(AttemptError.QuizNotFound(request.QuizId));
        }

        var existingAttempt = await _context.QuizAttempts
            .AsNoTracking()
            .Where(x => x.QuizId == request.QuizId && x.StudentId == request.StudentId && x.Status == QuizAttemptStatus.InProgress)
            .OrderByDescending(x => x.StartTime)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingAttempt is not null)
        {
            return Result.Failure<StartQuizResponse>(AttemptError.ExistingInProgress(existingAttempt.Id));
        }

        if (quiz.MaxAttempts.HasValue)
        {
            var attemptsCount = await _context.QuizAttempts
                .CountAsync(x => x.QuizId == request.QuizId && x.StudentId == request.StudentId, cancellationToken);

            if (attemptsCount >= quiz.MaxAttempts.Value)
            {
                return Result.Failure<StartQuizResponse>(AttemptError.LimitReached);
            }
        }

        var now = DateTime.UtcNow;
        var attempt = new QuizAttempt
        {
            StudentId = request.StudentId,
            QuizId = request.QuizId,
            StartTime = now,
            Deadline = now.AddMinutes(quiz.DurationMinutes),
            Status = QuizAttemptStatus.InProgress
        };

        await _context.QuizAttempts.AddAsync(attempt, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var shuffledQuestions = quiz.Questions
            .OrderBy(_ => Guid.NewGuid())
            .Select(q => new StartQuestionDto(
                q.Id,
                q.Text,
                q.Options
                    .OrderBy(_ => Guid.NewGuid())
                    .Select(o => new StartOptionDto(o.Id, o.Text))
                    .ToList()))
            .ToList();

        return Result.Success(new StartQuizResponse(
            attempt.Id,
            quiz.Id,
            attempt.StartTime,
            attempt.Deadline,
            shuffledQuestions));
    }
}

