using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Commands.ToggleDiplomaStatus;

public record ToggleDiplomaStatusCommand(Guid Id) : IRequest<Result>;

public class ToggleDiplomaStatusCommandHandler(IGenericRepository<Diploma> diplomaRepository) : IRequestHandler<ToggleDiplomaStatusCommand, Result>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository = diplomaRepository;

    public async Task<Result> Handle(ToggleDiplomaStatusCommand request, CancellationToken cancellationToken)
    {
        var affectedRows = await _diplomaRepository
            .GetQueryable()
            .Where(c => c.Id == request.Id)
            .ExecuteUpdateAsync(s => s
            .SetProperty(d => d.Status, DiplomaStatus.Published), cancellationToken);

        if (affectedRows == 0)
            return Result.Failure(DiplomaError.NotFound(request.Id));
        //await _diplomaRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}