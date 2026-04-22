using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Commands.ToggleDeleteStatus;

public record ToggleDeleteStatusCommand(Guid Id) : IRequest<Result>;

public class ToggleDeleteStatusCommandHandler(IGenericRepository<Diploma> diplomaRepository) : IRequestHandler<ToggleDeleteStatusCommand, Result>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository = diplomaRepository;

    public async Task<Result> Handle(ToggleDeleteStatusCommand request, CancellationToken cancellationToken)
    {
        var hasActiveEnrollments = await _diplomaRepository
            .GetQueryable()
            .Where(c => c.Id == request.Id)
            .Select(c => c.Enrollments.Any(e => !e.IsDeleted))
            .FirstOrDefaultAsync(cancellationToken);

        if (hasActiveEnrollments)
            return Result.Failure(DiplomaErrors.HasActiveEnrollments);

        var affectedRows = await _diplomaRepository
            .GetQueryable()
            .IgnoreQueryFilters()
            .Where(c => c.Id == request.Id)
            .ExecuteUpdateAsync(s => s
            .SetProperty(d => d.IsDeleted, x => !x.IsDeleted)
            .SetProperty(d => d.DeletedAt, x => x.IsDeleted ? DateTime.UtcNow : null),
            cancellationToken);

        if (affectedRows == 0)
            return Result.Failure(DiplomaErrors.NotFound(request.Id));

        return Result.Success();
    }
}