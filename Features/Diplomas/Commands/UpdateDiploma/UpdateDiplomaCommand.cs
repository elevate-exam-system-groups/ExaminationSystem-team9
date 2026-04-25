/* File Overview
 * File: UpdateDiplomaCommand.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.UpdateDiploma;

public record UpdateDiplomaCommand(
    Guid Id,
    string Title,
    string? Description
    ) : IRequest<Result>;

