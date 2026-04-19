using ExaminationSystem.Abstractions;

namespace ExaminationSystem.Errors.Authentication;

public static class UserError
{
    public static Error NotFound(Guid Id) =>
        new("User.NotFound", $"Student with ID '{Id}' was not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Email already registered.", StatusCodes.Status409Conflict);

    public static readonly Error NotFoundEnrolledDiplomas =
        new("User.NotFoundEnrolledDiplomas", "This user didn't enrolled in any diplomas", StatusCodes.Status404NotFound);

    public static readonly Error InvalidOrExpiredResetToken =
        new("User.InvalidOrExpiredResetToken", "The password reset token is invalid, expired, or has already been used.", StatusCodes.Status400BadRequest);
}
