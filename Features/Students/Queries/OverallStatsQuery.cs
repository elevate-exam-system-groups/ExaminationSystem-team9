using ExaminationSystem.Domain.DTOs.Student;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Students.Queries;

public record OverallStatsQuery(Guid UserId) : IRequest<OverallStatsResponse>;

public class OverallStatsQueryHandler(IGenericRepository<QuizAttempt> quizAttemptRepository) : IRequestHandler<OverallStatsQuery, OverallStatsResponse>
{
    private readonly IGenericRepository<QuizAttempt> _quizAttemptRepository = quizAttemptRepository;

    public async Task<OverallStatsResponse> Handle(OverallStatsQuery request, CancellationToken cancellationToken)
        =>
        await _quizAttemptRepository
            .GetQueryable()
            .Where(c => c.StudentId == request.UserId && c.Status == QuizAttemptStatus.Submitted)
            .GroupBy(c => 1)
            .Select(g => new OverallStatsResponse(
                g.Count(),
                g.Average(c => c.Score),
                g.Count(c => c.Passed == true) / g.Count() * 100.0,
                g.Sum(x => EF.Functions.DateDiffMinute(x.StartTime, x.SubmittedAt!.Value))
                ))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
}