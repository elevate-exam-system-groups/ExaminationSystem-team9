using ExaminationSystem.Features.Quizzes.Commands.CreateQuiz;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    }
}
