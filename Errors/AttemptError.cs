/* File Overview
 * File: AttemptError.cs
 * Purpose: Application error catalog: centralized, typed errors with HTTP status mappings.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;

namespace ExaminationSystem.Errors;

public static class AttemptError
{
    public static Error Unauthorized =>
        new("Attempt.Unauthorized", "Student identity is required.", StatusCodes.Status401Unauthorized);

    public static Error QuizNotFound(Guid quizId) =>
        new("Attempt.QuizNotFound", $"Quiz with id '{quizId}' was not found.", StatusCodes.Status404NotFound);

    public static Error AttemptNotFound(Guid attemptId) =>
        new("Attempt.NotFound", $"Attempt with id '{attemptId}' was not found.", StatusCodes.Status404NotFound);

    public static Error AccessDenied =>
        new("Attempt.AccessDenied", "You are not allowed to access this attempt.", StatusCodes.Status403Forbidden);

    public static Error LimitReached =>
        new("Attempt.LimitReached", "Maximum attempts reached for this quiz.", StatusCodes.Status403Forbidden);

    public static Error ExistingInProgress(Guid attemptId) =>
        new("Attempt.ExistingInProgress", $"An in-progress attempt already exists: '{attemptId}'.", StatusCodes.Status409Conflict);

    public static Error AlreadySubmitted =>
        new("Attempt.AlreadySubmitted", "Attempt is already submitted.", StatusCodes.Status409Conflict);

    public static Error InProgressResultsForbidden =>
        new("Attempt.InProgress", "Attempt is still in progress.", StatusCodes.Status403Forbidden);

    public static Error AttemptExpired =>
        new("Attempt.Expired", "Attempt deadline has passed.", StatusCodes.Status410Gone);

    public static Error QuestionNotInQuiz =>
        new("Attempt.QuestionNotInQuiz", "Question does not belong to this quiz attempt.", StatusCodes.Status400BadRequest);

    public static Error OptionInvalid =>
        new("Attempt.OptionInvalid", "Selected option does not belong to the given question.", StatusCodes.Status400BadRequest);
}

