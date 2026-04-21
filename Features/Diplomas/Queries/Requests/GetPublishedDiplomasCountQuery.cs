using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries.Requests;

public class GetPublishedDiplomasCountQuery() : IRequest<int>;

public class GetPublishedDiplomasCountQueryHandler(IGenericRepository<Diploma> diplomaRepository) : IRequestHandler<GetPublishedDiplomasCountQuery, int>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository = diplomaRepository;

    public async Task<int> Handle(GetPublishedDiplomasCountQuery request, CancellationToken cancellationToken) =>
        await _diplomaRepository.GetQueryable()
            .Where(c => c.Status == DiplomaStatus.Published)
            .CountAsync(cancellationToken);
}