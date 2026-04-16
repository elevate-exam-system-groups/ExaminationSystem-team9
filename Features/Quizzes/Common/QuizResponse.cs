namespace ExaminationSystem.Features.Quizzes.Common
{
    public record QuizResponse(
    Guid QuizId,
    Guid DiplomaId,
    string Title,
    int DurationMinutes,
    double PassScore,
    int? MaxAttempts,
    string? Instructions,
    string Status,
    int QuestionCount,
    DateTime CreatedAt
);
}
