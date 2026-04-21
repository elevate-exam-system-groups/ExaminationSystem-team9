namespace ExaminationSystem.DTOs.Authentication;

public record LoginRequest(
    string Email,
    string Password
);

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(l => l.Email).NotEmpty().EmailAddress();
        RuleFor(l => l.Password).NotEmpty();
    }
}