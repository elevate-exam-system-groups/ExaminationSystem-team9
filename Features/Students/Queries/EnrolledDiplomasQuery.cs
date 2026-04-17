using ExaminationSystem.Domain.DTOs.Student;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Students.Queries;

public record EnrolledDiplomasQuery(Guid UserId) : IRequest<List<EnrolledDiplomaResponse>>;

public class EnrolledDiplomasQueryHandler(IGenericRepository<Enrollment> EnrollmentRepository) : IRequestHandler<EnrolledDiplomasQuery, List<EnrolledDiplomaResponse>>
{
    private readonly IGenericRepository<Enrollment> _enrollmentRepository = EnrollmentRepository;

    public async Task<List<EnrolledDiplomaResponse>> Handle(EnrolledDiplomasQuery request, CancellationToken cancellationToken)
        =>
        await _enrollmentRepository
            .GetQueryable()
            .Where(c => c.StudentId == request.UserId)
            .ProjectToType<EnrolledDiplomaResponse>()
            .ToListAsync(cancellationToken);
}