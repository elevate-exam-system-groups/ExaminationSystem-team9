namespace ExaminationSystem.Domain.DTOs.Student;

public record RecentAttemptResponse(
    Guid Id,
    string QuizTitle,
    decimal? Score,
    bool? Passed,
    DateTime SubmittedAt
);