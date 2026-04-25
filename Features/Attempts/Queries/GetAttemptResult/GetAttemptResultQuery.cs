/* File Overview
 * File: GetAttemptResultQuery.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Attempts;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Errors;
using ExaminationSystem.Features.Attempts.Common;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptResult;

public record GetAttemptResultQuery(Guid AttemptId, Guid StudentId) : IRequest<Result<AttemptResultDto>>;

public class GetAttemptResultQueryHandler(ApplicationDbContext context, IAttemptLifecycleService lifecycleService)
    : IRequestHandler<GetAttemptResultQuery, Result<AttemptResultDto>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IAttemptLifecycleService _lifecycleService = lifecycleService;

    public async Task<Result<AttemptResultDto>> Handle(GetAttemptResultQuery request, CancellationToken cancellationToken)
    {
        var attempt = await _context.QuizAttempts
            .Include(x => x.Quiz)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Options)
            .Include(x => x.AttemptAnswers)
                .ThenInclude(x => x.SelectedOption)
            .Include(x => x.Result)
            .FirstOrDefaultAsync(x => x.Id == request.AttemptId, cancellationToken);

        if (attempt is null)
        {
            return Result.Failure<AttemptResultDto>(AttemptError.AttemptNotFound(request.AttemptId));
        }

        if (attempt.StudentId != request.StudentId)
        {
            return Result.Failure<AttemptResultDto>(AttemptError.AccessDenied);
        }

        if (_lifecycleService.IsExpired(attempt) && attempt.Status == QuizAttemptStatus.InProgress)
        {
            await _lifecycleService.FinalizeAttemptAsync(attempt, timedOut: true, cancellationToken);

            attempt = await _context.QuizAttempts
                .Include(x => x.Quiz)
                    .ThenInclude(x => x.Questions)
                        .ThenInclude(x => x.Options)
                .Include(x => x.AttemptAnswers)
                    .ThenInclude(x => x.SelectedOption)
                .Include(x => x.Result)
                .FirstAsync(x => x.Id == request.AttemptId, cancellationToken);
        }

        if (attempt.Status == QuizAttemptStatus.InProgress)
        {
            return Result.Failure<AttemptResultDto>(AttemptError.InProgressResultsForbidden);
        }

        var result = attempt.Result!;
        var breakdown = attempt.Quiz.Questions
            .OrderBy(x => x.OrderIndex)
            .Select(question =>
            {
                var answer = attempt.AttemptAnswers.FirstOrDefault(x => x.QuestionId == question.Id);
                var correctOption = question.Options.FirstOrDefault(x => x.IsCorrect);

                return new AttemptResultQuestionBreakdownDto(
                    question.Id,
                    question.Text,
                    answer?.SelectedOptionId,
                    answer?.SelectedOption?.Text,
                    correctOption?.Id,
                    correctOption?.Text,
                    answer?.IsCorrect ?? false,
                    question.Explanation);
            })
            .ToList();

        return Result.Success(new AttemptResultDto(
            attempt.Id,
            attempt.QuizId,
            attempt.Status,
            result.Score,
            result.Passed,
            result.CorrectAnswers,
            result.TotalQuestions,
            breakdown));
    }
}

