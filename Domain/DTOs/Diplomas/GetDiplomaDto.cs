/* File Overview
 * File: GetDiplomaDto.cs
 * Purpose: Supporting application source file within the Clean Architecture solution.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.DTOs.Diplomas;

public record GetDiplomaDto(
    Guid Id,
    string Title,
    string Description,
    DiplomaStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );

