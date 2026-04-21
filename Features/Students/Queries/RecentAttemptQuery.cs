using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.DTOs.Student;
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
            .Where(c => c.StudentId == request.UserId && c.Status == QuizAttemptStatus.Submitted)
            .OrderByDescending(c => c.SubmittedAt)
            .Select(c => new RecentAttemptResponse(
                c.Id,
                c.Quiz.Title,
                c.Score,
                c.Passed,
                c.SubmittedAt!.Value
                ))
            .ToListAsync(cancellationToken: cancellationToken);
}