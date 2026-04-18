using ExaminationSystem.Abstractions;
using ExaminationSystem.Features.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExaminationSystem.Controllers;

[Route("api/[controller]/dashboard")]
[ApiController]
[Authorize(Roles = "Student")]
public class StudentController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    [HttpGet("")]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new GetStudentDashboardQuery(Guid.Parse(userId!)), cancellationToken);

        //return Ok(result);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
