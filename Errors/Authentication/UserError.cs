using ExaminationSystem.Abstractions;

namespace ExaminationSystem.Errors.Authentication;

public static class UserError
{
    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Email already registered.", StatusCodes.Status409Conflict);

    public static readonly Error InvalidOrExpiredResetToken =
        new("User.InvalidOrExpiredResetToken", "The password reset token is invalid, expired, or has already been used.", StatusCodes.Status400BadRequest);
}
