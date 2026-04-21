using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.DTOs.Student;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Students.Queries;

public record EnrolledDiplomasQuery(Guid UserId) : IRequest<List<EnrolledDiplomaResponse>>;

public class EnrolledDiplomasQueryHandler(IGenericRepository<Enrollment> EnrollmentRepository) : IRequestHandler<EnrolledDiplomasQuery, List<EnrolledDiplomaResponse>>
{
    private readonly IGenericRepository<Enrollment> _enrollmentRepository = EnrollmentRepository;

    public async Task<List<EnrolledDiplomaResponse>> Handle(EnrolledDiplomasQuery request, CancellationToken cancellationToken)
    {
        var raw = await _enrollmentRepository
       .GetQueryable()
       .Where(c => c.StudentId == request.UserId)
       .Select(c => new
       {
           c.Id,
           DiplomaTitle = c.Diploma.Title,
           QuizCount = c.Diploma.Quizzes
               .Count(x => x.Status == QuizStatus.Published),
           CompletedQuizzes = c.Diploma.Quizzes
               .Count(q => q.Status == QuizStatus.Published
                        && q.QuizAttempts.Any(a =>
                               a.StudentId == request.UserId &&
                               a.Status == QuizAttemptStatus.Submitted)),
           LastActivityAt = c.Diploma.Quizzes
               .SelectMany(q => q.QuizAttempts)
               .Where(a => a.StudentId == request.UserId && a.SubmittedAt != null)
               .Max(a => (DateTime?)a.SubmittedAt)
       })
       .ToListAsync(cancellationToken);

        return raw.Select(x => new EnrolledDiplomaResponse(
            x.Id,
            x.DiplomaTitle,
            x.QuizCount,
            x.CompletedQuizzes,
            x.QuizCount == 0 ? 0.0 : (double)x.CompletedQuizzes / x.QuizCount * 100.0,
            x.LastActivityAt
            )).ToList();
    }
}