/* File Overview
 * File: GetAttemptTimerQuery.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Attempts;
using ExaminationSystem.Errors;
using ExaminationSystem.Features.Attempts.Common;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptTimer;

public record GetAttemptTimerQuery(Guid AttemptId, Guid StudentId) : IRequest<Result<TimerResponse>>;

public class GetAttemptTimerQueryHandler(ApplicationDbContext context, IAttemptLifecycleService lifecycleService)
    : IRequestHandler<GetAttemptTimerQuery, Result<TimerResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IAttemptLifecycleService _lifecycleService = lifecycleService;

    public async Task<Result<TimerResponse>> Handle(GetAttemptTimerQuery request, CancellationToken cancellationToken)
    {
        var attempt = await _context.QuizAttempts
            .FirstOrDefaultAsync(x => x.Id == request.AttemptId, cancellationToken);

        if (attempt is null)
        {
            return Result.Failure<TimerResponse>(AttemptError.AttemptNotFound(request.AttemptId));
        }

        if (attempt.StudentId != request.StudentId)
        {
            return Result.Failure<TimerResponse>(AttemptError.AccessDenied);
        }

        if (_lifecycleService.IsExpired(attempt) && attempt.Status == Domain.Enums.QuizAttemptStatus.InProgress)
        {
            await _lifecycleService.FinalizeAttemptAsync(attempt, timedOut: true, cancellationToken);
            return Result.Failure<TimerResponse>(AttemptError.AttemptExpired);
        }

        var remaining = attempt.Deadline <= DateTime.UtcNow ? 0 : (int)Math.Ceiling((attempt.Deadline - DateTime.UtcNow).TotalSeconds);
        return Result.Success(new TimerResponse(attempt.Id, remaining));
    }
}

