namespace ExaminationSystem.Domain.DTOs.Student;

public record StudentDashboardResponse(
    StudentResponse Student,
    List<EnrolledDiplomaResponse> EnrolledDiplomas,
    List<RecentAttemptResponse> RecentAttempts,
    OverallStatsResponse OverallStats
);