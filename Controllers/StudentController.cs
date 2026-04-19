using ExaminationSystem.Abstractions;
using ExaminationSystem.Features.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[Route("api/[controller]/dashboard")]
[ApiController]
//[Authorize(Roles = "Student")]
public class StudentController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    [HttpGet("")]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        //var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new GetStudentDashboardOrchestrator(Guid.Parse("cbb9db8a-4370-4d03-ac3d-08de986e3eeb")), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
