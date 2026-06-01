namespace ExaminationSystem.DTOs.Authentication;

public record ConfirmationEmailRequest(
    string Email,
    string OtpCode
    );

public class ConfirmationEmailRequestValidator : AbstractValidator<ConfirmationEmailRequest>
{
    public ConfirmationEmailRequestValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();

        RuleFor(c => c.OtpCode).NotEmpty();
    }
}