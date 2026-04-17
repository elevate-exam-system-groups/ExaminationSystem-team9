using ExaminationSystem.Abstractions;
using ExaminationSystem.Features.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[Route("api/[controller]/dashboard")]
[ApiController]
//[Authorize(Roles = "Student")]
public class StudentController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetStudentDashboardQuery(Guid.Parse("cbb9db8a-4370-4d03-ac3d-08de986e3eeb")), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
