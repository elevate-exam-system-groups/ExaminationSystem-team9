using ExaminationSystem.Abstractions;
using ExaminationSystem.Features.Quizzes.Commands.CreateQuiz;
using ExaminationSystem.Features.Quizzes.Commands.PublishQuiz;
using ExaminationSystem.Features.Quizzes.Commands.UnpublishQuiz;
using ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{
    [ApiController]
    [Route("api/")]
    public class QuizzesController : Controller
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

        [HttpPut("admin/quizzes/{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateQuizCommand command)
        {
            command = command with { Id = id };
            var result = await _mediator.Send(command);
            return result.IsSuccess
                ? NoContent()
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

        [HttpPatch("admin/quizzes/{quizId}/unpublish")]
        public async Task<IActionResult> Unpublish([FromRoute] Guid quizId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UnpublishQuizCommand(quizId), cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }
    }
}
