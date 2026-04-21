namespace ExaminationSystem.DTOs.Admin;

public record AdminStatsResponse(
    int TotalUsers,
    int ActivateUsersToday,
    int TotalDiplomas,
    int TotalQuizzes//,
                    //int TotalAttempts,
                    //double AvgPassRate
);
