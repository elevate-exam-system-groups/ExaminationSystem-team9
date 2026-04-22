namespace ExaminationSystem.DTOs.Student;

public record StudentResponse(Guid Id,
    string FullName,
    string Email
    );
