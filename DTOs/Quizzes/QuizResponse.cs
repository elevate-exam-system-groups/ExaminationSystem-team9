namespace ExaminationSystem.DTOs.Quizzes
{
// Location: Domain/DTOs/Quizzes/QuizResponse.cs
public record QuizResponse
{
    public Guid QuizId { get; init; }
    public Guid DiplomaId { get; init; }
    public string Title { get; init; } = default!;
    public int DurationMinutes { get; init; }
    public double PassScore { get; init; }
    public int? MaxAttempts { get; init; }
    public string Status { get; init; } = default!;
    public int QuestionCount { get; init; }
    public DateTime CreatedAt { get; init; }
}
}

