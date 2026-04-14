using ExaminationSystem.Abstractions;

namespace ExaminationSystem.Errors.Authentication;

public static class UserError
{
    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Email already registered.", StatusCodes.Status409Conflict);
}
