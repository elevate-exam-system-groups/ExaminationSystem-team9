using ExaminationSystem.Abstractions.Constants;
using ExaminationSystem.Features.Quizzes.Queries;
using Microsoft.AspNetCore.Authorization;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/")]
public class QuizzesController : ControllerBase
{
    private readonly ISender _mediator;

    public QuizzesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("admin/quizzes")]
    public async Task<IActionResult> Create([FromBody] CreateQuizCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? StatusCode(201, result)
            : BadRequest(result);
    }

    [HttpPost("admin/quizzes/{quizId}/questions")]
    public async Task<IActionResult> AddQuestion(
        [FromRoute] Guid quizId,
        [FromBody] AddQuestionCommand command,
        CancellationToken cancellationToken)
    {
        command = command with { QuizId = quizId };
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result.Value)
            : result.ToProblem();
    }

    [HttpPut("admin/quizzes/{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateQuizCommand command)
    {
        command = command with { Id = id };
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("admin/questions/{questionId}")]
    public async Task<IActionResult> UpdateQuestion(
        [FromRoute] Guid questionId,
        [FromBody] UpdateQuestionCommand command,
        CancellationToken cancellationToken)
    {
        command = command with { QuestionId = questionId };
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPatch("admin/quizzes/{quizId}/publish")]
    public async Task<IActionResult> Publish([FromRoute] Guid quizId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new PublishQuizCommand(quizId), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpPatch("admin/quizzes/{quizId}/unpublish")]
    public async Task<IActionResult> Unpublish([FromRoute] Guid quizId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UnpublishQuizCommand(quizId), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [Authorize]
    [HttpGet("diplomas/{diplomaId}/quizzes")]
    public async Task<IActionResult> GetDiplomaQuizzes([FromRoute] Guid diplomaId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDiplomaQuizzesOrchestrator(diplomaId), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
