using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.DTOs.Diplomas;

public record GetDiplomaDto(
    Guid Id,
    string Title,
    string Description,
    DiplomaStatus Status,
    DateTime DeletedAt
    );
