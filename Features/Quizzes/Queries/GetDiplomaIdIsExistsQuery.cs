using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Features.Quizzes.Queries;

public record GetDiplomaIdIsExistsQuery(Guid Id) : IRequest<bool>;

public class GetDiplomaIdIsExistsQueryHandler(IGenericRepository<Diploma> diplomaRepository) : IRequestHandler<GetDiplomaIdIsExistsQuery, bool>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository = diplomaRepository;

    public async Task<bool> Handle(GetDiplomaIdIsExistsQuery request, CancellationToken cancellationToken) =>
        await _diplomaRepository
            .GetQueryable()
            .AnyAsync(c => c.Id == request.Id && c.Status == DiplomaStatus.Published, cancellationToken);
}