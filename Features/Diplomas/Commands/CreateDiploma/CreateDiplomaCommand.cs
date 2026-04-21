using ExaminationSystem.Abstractions;
using ExaminationSystem.DTOs.Diplomas;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.CreateDiploma;

public record CreateDiplomaCommand(
    string Title,
    string? Description
    ) : IRequest<Result<GetDiplomaDto>>;
