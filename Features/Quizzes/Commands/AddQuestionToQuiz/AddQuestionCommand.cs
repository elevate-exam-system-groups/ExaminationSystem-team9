using ExaminationSystem.Abstractions;
using ExaminationSystem.DTOs.Questions;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.AddQuestionToQuiz;

public record AddQuestionCommand : IRequest<Result<AddQuestionResponse>>
{
    public Guid QuizId { get; init; }
    public string Text { get; init; } = default!;
    public int OrderIndex { get; init; }
    public string? Explanation { get; init; }
    public List<AddQuestionOptionRequest> Options { get; init; } = [];
}

public record AddQuestionOptionRequest
{
    public string Text { get; init; } = default!;
    public bool IsCorrect { get; init; }
}
