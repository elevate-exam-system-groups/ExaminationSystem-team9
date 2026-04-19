using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Commands.UpdateDiploma;

public class UpdateDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepository) : IRequestHandler<UpdateDiplomaCommand, Result>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository = diplomaRepository;

    public async Task<Result> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
    {
        var affectedRow = await _diplomaRepository
            .GetQueryable()
            .Where(c => c.Id == request.Id)
            .ExecuteUpdateAsync(s => s
            .SetProperty(d => d.Title, request.Title)
            .SetProperty(d => d.Description, request.Description)
            .SetProperty(d => d.UpdatedAt, DateTime.UtcNow),
            cancellationToken);

        if (affectedRow == 0)
            return Result.Failure(DiplomaError.NotFound(request.Id));

        return Result.Success();
    }
}
