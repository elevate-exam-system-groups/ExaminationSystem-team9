using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Admin;
using ExaminationSystem.Features.Diplomas.Queries.Requests;
using ExaminationSystem.Features.Quizzes.Queries;
using MediatR;

namespace ExaminationSystem.Features.Admin.Queries;

public record GetAdminDashboardStatsOrchestrator() : IRequest<Result<AdminStatsResponse>>;

public class GetAdminDashboardStatsOrchestratorHandler(IMediator mediator) : IRequestHandler<GetAdminDashboardStatsOrchestrator, Result<AdminStatsResponse>>
{
    private readonly IMediator _mediator = mediator;

    public async Task<Result<AdminStatsResponse>> Handle(GetAdminDashboardStatsOrchestrator request, CancellationToken cancellationToken)
    {
        var totalUsers = await _mediator.Send(new GetUsersCountQuery(), cancellationToken);

        var activateUsersToday = await _mediator.Send(new GetActiveUsersTodayCountQuery(), cancellationToken);

        var totalDiplomas = await _mediator.Send(new GetPublishedDiplomasCountQuery(), cancellationToken);

        var totalQuizzes = await _mediator.Send(new GetPublishedQuizzesCountQuery(), cancellationToken);

        var response = new AdminStatsResponse(totalUsers, activateUsersToday, totalDiplomas, totalQuizzes);

        return Result.Success(response);
    }
}