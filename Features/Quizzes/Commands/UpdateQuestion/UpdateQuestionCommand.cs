using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Questions;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuestion;

public record UpdateQuestionCommand : IRequest<Result<UpdateQuestionResponse>>
{
    public Guid QuestionId { get; init; }
    public string Text { get; init; } = default!;
    public int OrderIndex { get; init; }
    public string? Explanation { get; init; }
    public List<UpdateQuestionOptionRequest> Options { get; init; } = [];
}

public record UpdateQuestionOptionRequest
{
    public Guid OptionId { get; init; }
    public string Text { get; init; } = default!;
    public bool IsCorrect { get; init; }
}
