namespace ExaminationSystem.Domain.DTOs.Student;

public record EnrolledDiplomaResponse(int Id,
    string DiplomaTitle,
    int QuizCount,
    int CompletedQuizzes,
    double ProgressPercentage,
    DateTime? LastActivityAt
    );
