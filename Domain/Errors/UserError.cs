using ExaminationSystem.Abstractions;

namespace ExaminationSystem.Domain.Errors;

public static class UserError
{
    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Email already registered.", StatusCodes.Status409Conflict);
}
