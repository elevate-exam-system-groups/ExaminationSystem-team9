/* File Overview
 * File: UserError.cs
 * Purpose: Application error catalog: centralized, typed errors with HTTP status mappings.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;

namespace ExaminationSystem.Errors.Authentication;

public static class UserError
{
    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Email already registered.", StatusCodes.Status409Conflict);
}

