/* File Overview
 * File: AttemptsController.cs
 * Purpose: API controller layer: exposes HTTP endpoints, handles request/response mapping, and delegates business logic to MediatR handlers.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Attempts;
using ExaminationSystem.Domain.Interfaces.Authentication;
using ExaminationSystem.Errors;
using ExaminationSystem.Features.Attempts.Commands.SubmitAnswer;
using ExaminationSystem.Features.Attempts.Commands.SubmitAttempt;
using ExaminationSystem.Features.Attempts.Queries.GetAttemptResult;
using ExaminationSystem.Features.Attempts.Queries.GetAttemptTimer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[Route("api/attempts")]
[ApiController]
public class AttemptsController(IMediator mediator, ICurrentUserService currentUserService) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpPost("{id:guid}/answer")]
    public async Task<IActionResult> SubmitAnswer([FromRoute] Guid id, [FromBody] SubmitAnswerRequest request, CancellationToken cancellationToken)
    {
        var studentId = _currentUserService.GetCurrentUserId();
        if (!studentId.HasValue)
        {
            return Result.Failure(AttemptError.Unauthorized).ToProblem();
        }

        var result = await _mediator.Send(
            new SubmitAnswerCommand(id, studentId.Value, request.QuestionId, request.SelectedOptionId),
            cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpGet("{id:guid}/timer")]
    public async Task<IActionResult> GetTimer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var studentId = _currentUserService.GetCurrentUserId();
        if (!studentId.HasValue)
        {
            return Result.Failure(AttemptError.Unauthorized).ToProblem();
        }

        var result = await _mediator.Send(new GetAttemptTimerQuery(id, studentId.Value), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<IActionResult> Submit([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var studentId = _currentUserService.GetCurrentUserId();
        if (!studentId.HasValue)
        {
            return Result.Failure(AttemptError.Unauthorized).ToProblem();
        }

        var result = await _mediator.Send(new SubmitAttemptCommand(id, studentId.Value), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}/results")]
    public async Task<IActionResult> GetResults([FromRoute] Guid id, CancellationToken cancellationToken)
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

