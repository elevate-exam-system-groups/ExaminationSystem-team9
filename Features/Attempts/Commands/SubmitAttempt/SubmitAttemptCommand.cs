/* File Overview
 * File: SubmitAttemptCommand.cs
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

namespace ExaminationSystem.Features.Attempts.Commands.SubmitAttempt;

public record SubmitAttemptCommand(Guid AttemptId, Guid StudentId) : IRequest<Result<SubmitAttemptResponse>>;

public class SubmitAttemptCommandHandler(ApplicationDbContext context, IAttemptLifecycleService lifecycleService)
    : IRequestHandler<SubmitAttemptCommand, Result<SubmitAttemptResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IAttemptLifecycleService _lifecycleService = lifecycleService;

    public async Task<Result<SubmitAttemptResponse>> Handle(SubmitAttemptCommand request, CancellationToken cancellationToken)
    {
        var attempt = await _context.QuizAttempts
            .FirstOrDefaultAsync(x => x.Id == request.AttemptId, cancellationToken);

        if (attempt is null)
        {
            return Result.Failure<SubmitAttemptResponse>(AttemptError.AttemptNotFound(request.AttemptId));
        }

        if (attempt.StudentId != request.StudentId)
        {
            return Result.Failure<SubmitAttemptResponse>(AttemptError.AccessDenied);
        }

        if (attempt.Status == QuizAttemptStatus.Submitted)
        {
            return Result.Failure<SubmitAttemptResponse>(AttemptError.AlreadySubmitted);
        }

        var timedOut = _lifecycleService.IsExpired(attempt);
        var result = await _lifecycleService.FinalizeAttemptAsync(attempt, timedOut, cancellationToken);
        return Result.Success(result);
    }
}

