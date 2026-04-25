/* File Overview
 * File: UpdateDiplomaCommandValidator.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using FluentValidation;

namespace ExaminationSystem.Features.Diplomas.Commands.UpdateDiploma;

public class UpdateDiplomaCommandValidator : AbstractValidator<UpdateDiplomaCommand>
{
    public UpdateDiplomaCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().
            Length(3, 100);

        RuleFor(c => c.Description)
            .MaximumLength(500);
    }
}

