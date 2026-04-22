using ExaminationSystem.Domain.Entities;
using ExaminationSystem.DTOs.Quizzes;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Queries;

public record GetDiplomaQuizzesQuery(Guid DiplomaId) : IRequest<IEnumerable<QuizResponse>>;

public class GetDiplomaQuizzesHandler(IGenericRepository<Quiz> quizRepository) : IRequestHandler<GetDiplomaQuizzesQuery, IEnumerable<QuizResponse>>
{
    private readonly IGenericRepository<Quiz> _quizRepository = quizRepository;

    public async Task<IEnumerable<QuizResponse>> Handle(GetDiplomaQuizzesQuery request, CancellationToken cancellationToken) =>
        await _quizRepository
            .GetQueryable()
            .Where(c => c.DiplomaId == request.DiplomaId)
            .ProjectToType<QuizResponse>()
            .ToListAsync(cancellationToken: cancellationToken);
}