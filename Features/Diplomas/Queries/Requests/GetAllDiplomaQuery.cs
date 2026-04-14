using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Common;
using ExaminationSystem.Domain.DTOs.Diplomas;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Queries.Requests;

public record GetAllDiplomaQuery(RequestFilters Filters) : IRequest<PaginatedList<GetDiplomaDto>>;

public class GetAllDiplomaQueryHandler(IGenericRepository<Diploma> diplomaRepository) : IRequestHandler<GetAllDiplomaQuery, PaginatedList<GetDiplomaDto>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository = diplomaRepository;

    public async Task<PaginatedList<GetDiplomaDto>> Handle(GetAllDiplomaQuery request, CancellationToken cancellationToken)
    {
        var query = _diplomaRepository
            .GetQueryable()
            .Where(c => c.Status == DiplomaStatus.Published)
            .ProjectToType<GetDiplomaDto>();

        var diplomas = await PaginatedList<GetDiplomaDto>.
            CreateAsync(query, request.Filters.PageNumber, request.Filters.PageSize, cancellationToken);

        return diplomas;
    }
}