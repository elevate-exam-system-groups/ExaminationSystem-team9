using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Queries;

public record GetPublishedQuizzesCountQuery() : IRequest<int>;

public class GetPublishedQuizzesCountQueryHandler(IGenericRepository<Quiz> QuizRepository) : IRequestHandler<GetPublishedQuizzesCountQuery, int>
{
    private readonly IGenericRepository<Quiz> _quizRepository = QuizRepository;

    public async Task<int> Handle(GetPublishedQuizzesCountQuery request, CancellationToken cancellationToken) =>
        await _quizRepository
            .GetQueryable()
            .Where(c => c.Status == QuizStatus.Published)
            .CountAsync(cancellationToken);
}