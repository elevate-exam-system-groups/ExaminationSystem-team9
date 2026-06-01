using ExaminationSystem.DTOs.Student;
using MediatR;

namespace ExaminationSystem.Features.Students.Queries;

public record GetStudentQuery(Guid UserId) : IRequest<StudentResponse>;

public class GetStudentQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetStudentQuery, StudentResponse>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<StudentResponse> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        =>
        await _userManager.Users
            .Where(x => x.Id == request.UserId)
            .ProjectToType<StudentResponse>() 
            .FirstOrDefaultAsync(cancellationToken);
}