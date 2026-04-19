using ExaminationSystem.Abstractions;

namespace ExaminationSystem.Errors;

public static class QuestionError
{
    public static Error NotFound(Guid id) =>
        new("Question.NotFound", $"Question with id '{id}' was not found.", StatusCodes.Status404NotFound);

    public static Error InvalidOptions(Guid questionId) =>
        new("Question.InvalidOptions", $"Submitted options do not match the existing options for question '{questionId}'.", StatusCodes.Status400BadRequest);
}
