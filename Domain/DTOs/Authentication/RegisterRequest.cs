/* File Overview
 * File: RegisterRequest.cs
 * Purpose: Supporting application source file within the Clean Architecture solution.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions.Constants;
using FluentValidation;

namespace ExaminationSystem.Domain.DTOs.Authentication;

public record RegisterRequest(
    string Email,
    string Password,
    string FullName
    );

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(c => c.Password)
            .Matches(RegexPattern.Password)
             .WithMessage("password should be at least 8 digits and contains upperCase, lowercase, NonAlphanumeric");

        RuleFor(c => c.FullName)
            .NotEmpty()
            .Length(3, 100);
    }
}
