namespace ExaminationSystem.DTOs.Questions;

public record UpdateQuestionResponse
{
    public Guid QuestionId { get; init; }
    public DateTime UpdatedAt { get; init; }
}
