using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Student;
using ExaminationSystem.Errors.Authentication;
using MediatR;

namespace ExaminationSystem.Features.Students.Queries;

public record GetStudentDashboardOrchestrator(Guid UserId) : IRequest<Result<StudentDashboardResponse>>;

public class StudentDashboardResponseHandler(IMediator mediator) : IRequestHandler<GetStudentDashboardOrchestrator, Result<StudentDashboardResponse>>
{
    private readonly IMediator _mediator = mediator;

    public async Task<Result<StudentDashboardResponse>> Handle(GetStudentDashboardOrchestrator request, CancellationToken cancellationToken)
    {
        var student = await _mediator.Send(new GetStudentQuery(request.UserId), cancellationToken);

        if (student is null)
            return Result.Failure<StudentDashboardResponse>(UserError.NotFound(request.UserId));

        var enrolledDiplomas = await _mediator.Send(new EnrolledDiplomasQuery(request.UserId), cancellationToken);

        var recentAttempt = await _mediator.Send(new RecentAttemptQuery(request.UserId), cancellationToken);

        var overAllStats = await _mediator.Send(new OverallStatsQuery(request.UserId), cancellationToken);

        var response = new StudentDashboardResponse(student, enrolledDiplomas, recentAttempt, overAllStats);

        return Result.Success<StudentDashboardResponse>(response);
    }
}