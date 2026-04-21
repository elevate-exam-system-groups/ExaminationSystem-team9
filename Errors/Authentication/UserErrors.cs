namespace ExaminationSystem.Errors.Authentication;

public static class UserErrors
{
    public static Error NotFound(Guid Id) =>
        new("User.NotFound", $"Student with ID '{Id}' was not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Email already registered.", StatusCodes.Status409Conflict);

    public static readonly Error NotFoundEnrolledDiplomas =
        new("User.NotFoundEnrolledDiplomas", "This user didn't enrolled in any diplomas", StatusCodes.Status404NotFound);

    public static readonly Error InvalidOrExpiredResetToken =
        new("User.InvalidOrExpiredResetToken", "The password reset token is invalid, expired, or has already been used.", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidCredentials =
    new("User.InvalidCredentials", "Invalid Email/Password", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidToken =
    new("Token.InvalidToken", "Invalid jwt Token", StatusCodes.Status401Unauthorized);

    public static readonly Error EmailNotConfirmed =
    new("Email.EmailNotConfirmed", "Email is not confirmed.", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidCode =
    new("User.InvalidCode", "Invalid Code.", StatusCodes.Status401Unauthorized);

    public static readonly Error EmailIsConfirmed =
    new("Email.EmailIsConfirmed", "Email is already confirmed.", StatusCodes.Status401Unauthorized);
}
