using ExaminationSystem.Domain.DTOs.Student;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Students.Queries;

public record RecentAttemptQuery(Guid UserId) : IRequest<List<RecentAttemptResponse>>;

public class RecentAttemptQueryHandler(IGenericRepository<QuizAttempt> quizAttemptRepository) : IRequestHandler<RecentAttemptQuery, List<RecentAttemptResponse>>
{
    private readonly IGenericRepository<QuizAttempt> _quizAttemptRepository = quizAttemptRepository;

    public async Task<List<RecentAttemptResponse>> Handle(RecentAttemptQuery request, CancellationToken cancellationToken)
        =>
        await _quizAttemptRepository
            .GetQueryable()
            .Where(c => c.StudentId == request.UserId)
            .OrderByDescending(c => c.CreatedAt)
            .ProjectToType<RecentAttemptResponse>()
            .ToListAsync(cancellationToken: cancellationToken);
}