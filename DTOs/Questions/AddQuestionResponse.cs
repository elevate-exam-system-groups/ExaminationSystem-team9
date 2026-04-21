namespace ExaminationSystem.DTOs.Questions;

public record AddQuestionResponse
{
    public Guid QuestionId { get; init; }
    public Guid QuizId { get; init; }
    public string Text { get; init; } = default!;
    public int OrderIndex { get; init; }
    public int OptionCount { get; init; }
    public DateTime CreatedAt { get; init; }
}
