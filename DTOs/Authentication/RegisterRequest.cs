using ExaminationSystem.Abstractions.Constants;
using FluentValidation;

namespace ExaminationSystem.DTOs.Authentication;

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