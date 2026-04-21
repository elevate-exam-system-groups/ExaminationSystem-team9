using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.DTOs.Diplomas;
using ExaminationSystem.Errors;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries.Requests;

public record GetDiplomaByIdQuery(Guid Id) : IRequest<Result<GetDiplomaDto>>;

public class GetDiplomaByIdQueryHandler(IGenericRepository<Diploma> DiplomaRepository) : IRequestHandler<GetDiplomaByIdQuery, Result<GetDiplomaDto>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository = DiplomaRepository;

    public async Task<Result<GetDiplomaDto>> Handle(GetDiplomaByIdQuery request, CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository
        .GetQueryable()
        .Where(c => c.Id == request.Id)
        .ProjectToType<GetDiplomaDto>()
        .FirstOrDefaultAsync(cancellationToken);

        if (diploma is null)
            return Result.Failure<GetDiplomaDto>(DiplomaError.NotFound(request.Id));

        return Result.Success(diploma);
    }
}