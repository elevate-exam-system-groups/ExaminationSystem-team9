namespace ExaminationSystem.Domain.DTOs.Student;

public record EnrolledDiplomaResponse(Guid Id,
    string DiplomaTitle,
    int QuizCount,
    int CompletedQuizzes
    //string progressPercentage
    //string LastActivityAt
    );
