using ExaminationSystem.Abstractions;

namespace ExaminationSystem.Errors;

public static class QuizError
{
    public static Error NotFound(Guid id) =>
        new("Quiz.NotFound", $"Quiz with id '{id}' was not found.", StatusCodes.Status404NotFound);

    public static Error AlreadyPublished(Guid id) =>
        new("Quiz.AlreadyPublished", $"Quiz with id '{id}' is already published.", StatusCodes.Status409Conflict);

    public static Error AlreadyDraft(Guid id) =>
        new("Quiz.AlreadyDraft", $"Quiz with id '{id}' is already unpublished.", StatusCodes.Status409Conflict);

    public static Error HasNoQuestions(Guid id) =>
        new("Quiz.HasNoQuestions", $"Quiz with id '{id}' must contain at least one question before publishing.", StatusCodes.Status400BadRequest);

    public static Error QuestionHasTooFewOptions(Guid questionId) =>
        new("Quiz.QuestionHasTooFewOptions", $"Question with id '{questionId}' must contain at least two options before publishing.", StatusCodes.Status400BadRequest);

    public static Error QuestionMustHaveOneCorrectOption(Guid questionId) =>
        new("Quiz.QuestionMustHaveOneCorrectOption", $"Question with id '{questionId}' must have exactly one correct option before publishing.", StatusCodes.Status400BadRequest);

    public static Error HasInProgressAttempts(Guid id) =>
        new("Quiz.HasInProgressAttempts", $"Quiz with id '{id}' cannot be unpublished while in-progress attempts exist.", StatusCodes.Status409Conflict);
}
