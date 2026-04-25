/* File Overview
 * File: QuizzesController.cs
 * Purpose: API controller layer: exposes HTTP endpoints, handles request/response mapping, and delegates business logic to MediatR handlers.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Interfaces.Authentication;
using ExaminationSystem.Errors;
using ExaminationSystem.Features.Attempts.Commands.StartQuiz;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Controllers;

[Route("api/quizzes")]
[ApiController]
public class QuizzesController(
    IMediator mediator,
    ICurrentUserService currentUserService,
    ApplicationDbContext context) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly ApplicationDbContext _context = context;

    [HttpPost("{id:guid}/start")]
    public async Task<IActionResult> StartQuiz([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var studentId = _currentUserService.GetCurrentUserId();
        if (!studentId.HasValue)
        {
            return Result.Failure(AttemptError.Unauthorized).ToProblem();
        }

        var result = await _mediator.Send(new StartQuizCommand(id, studentId.Value), cancellationToken);

        if (!result.IsSuccess && result.Error.Code == "Attempt.ExistingInProgress")
        {
            var existingAttemptId = await _context.QuizAttempts
                .AsNoTracking()
                .Where(x => x.QuizId == id && x.StudentId == studentId.Value && x.Status == Domain.Enums.QuizAttemptStatus.InProgress)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingAttemptId != Guid.Empty)
            {
                return Conflict(new { attempt_id = existingAttemptId, error = result.Error.Code });
            }
        }

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}

