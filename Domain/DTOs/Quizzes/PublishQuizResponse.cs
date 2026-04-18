namespace ExaminationSystem.Domain.DTOs.Quizzes;

public record PublishQuizResponse
{
    public Guid QuizId { get; init; }
    public string Status { get; init; } = default!;
    public DateTime? PublishedAt { get; init; }
}
