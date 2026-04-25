/* File Overview
 * File: QuizAttemptStatus.cs
 * Purpose: Supporting application source file within the Clean Architecture solution.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

namespace ExaminationSystem.Domain.Enums;

public enum QuizAttemptStatus
{
    InProgress = 1,
    Submitted = 2,
    TimedOut = 3
}

