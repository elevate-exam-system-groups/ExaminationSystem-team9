/* File Overview
 * File: StudentAttemptsController.cs
 * Purpose: API controller layer: exposes HTTP endpoints, handles request/response mapping, and delegates business logic to MediatR handlers.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Interfaces.Authentication;
using ExaminationSystem.Errors;
using ExaminationSystem.Features.Attempts.Queries.GetAttemptResult;
using ExaminationSystem.Features.Attempts.Queries.GetStudentAttempts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[Route("api/student/attempts")]
[ApiController]
public class StudentAttemptsController(IMediator mediator, ICurrentUserService currentUserService) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpGet]
    public async Task<IActionResult> GetAttempts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? quiz_id = null,
        [FromQuery] Guid? diploma_id = null,
        CancellationToken cancellationToken = default)
    {
        var studentId = _currentUserService.GetCurrentUserId();
        if (!studentId.HasValue)
        {
            return Result.Failure(AttemptError.Unauthorized).ToProblem();
        }

        var result = await _mediator.Send(
            new GetStudentAttemptsQuery(studentId.Value, pageNumber, pageSize, quiz_id, diploma_id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAttemptDetails([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var studentId = _currentUserService.GetCurrentUserId();
        if (!studentId.HasValue)
        {
            return Result.Failure(AttemptError.Unauthorized).ToProblem();
        }

        var result = await _mediator.Send(new GetAttemptResultQuery(id, studentId.Value), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}

