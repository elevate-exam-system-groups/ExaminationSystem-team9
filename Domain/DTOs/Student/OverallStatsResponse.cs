namespace ExaminationSystem.Domain.DTOs.Student;

public record OverallStatsResponse(
    int TotalQuizzes,
    decimal? AvgScore,
    double PassRate,
    int TotalTimeSpentMinutes
    );