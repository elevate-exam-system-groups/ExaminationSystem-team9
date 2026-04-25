/* File Overview
 * File: DiplomaError.cs
 * Purpose: Application error catalog: centralized, typed errors with HTTP status mappings.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;

namespace ExaminationSystem.Errors;

public static class DiplomaError
{
    public static Error NotFound(Guid Id) =>
        new("Diploma.NotFound", $"Diploma with ID '{Id}' was not found", StatusCodes.Status404NotFound);
    public static Error HasActiveEnrollments =>
        new("Diploma.HasActiveEnrollments", $"We found user enrollment in this diploma", StatusCodes.Status409Conflict);
}

