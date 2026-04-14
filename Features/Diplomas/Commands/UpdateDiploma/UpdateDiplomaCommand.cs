using ExaminationSystem.Abstractions;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.UpdateDiploma;

public record UpdateDiplomaCommand(
    Guid Id,
    string Title,
    string? Description
    ) : IRequest<Result>;
