using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.DTOs.Diplomas;

public record GetDiplomaDto(
    Guid Id,
    string Title,
    string Description,
    DiplomaStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );
