using ExaminationSystem.Features.Admin.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles =DefaultRoles.Admin)]
public class AdminController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("stats")]
    public async Task<IActionResult> Stats(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAdminDashboardStatsOrchestrator(), cancellationToken);

        return Ok(result.Value);
    }
}
