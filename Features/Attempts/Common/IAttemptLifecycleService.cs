/* File Overview
 * File: IAttemptLifecycleService.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.DTOs.Attempts;
using ExaminationSystem.Domain.Entities;

namespace ExaminationSystem.Features.Attempts.Common;

public interface IAttemptLifecycleService
{
    bool IsExpired(QuizAttempt attempt);
    Task<SubmitAttemptResponse> FinalizeAttemptAsync(QuizAttempt attempt, bool timedOut, CancellationToken cancellationToken);
}

