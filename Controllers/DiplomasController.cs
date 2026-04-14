using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Common;
using ExaminationSystem.Features.Diplomas.Commands.CreateDiploma;
using ExaminationSystem.Features.Diplomas.Commands.ToggleDeleteStatus;
using ExaminationSystem.Features.Diplomas.Commands.ToggleDiplomaStatus;
using ExaminationSystem.Features.Diplomas.Commands.UpdateDiploma;
using ExaminationSystem.Features.Diplomas.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{
    [Route("api/")]
    [ApiController]
    public class DiplomasController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("diplomas")]
        public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllDiplomaQuery(filters), cancellationToken);

            return Ok(result);
        }

        [HttpGet("admin/diplomas/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetDiplomaByIdQuery(Id), cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("admin/diplomas")]
        public async Task<IActionResult> Create([FromBody] CreateDiplomaCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return result.IsSuccess ?
                CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("admin/diplomas/{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateDiplomaCommand command, CancellationToken cancellationToken)
        {
            command = command with { Id = id };

            var result = await _mediator.Send(command, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("admin/diploma/toggleStatus/{id}")]
        public async Task<IActionResult> ToggleStatus([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ToggleDiplomaStatusCommand(id), cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("admin/diploma/soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ToggleDeleteStatusCommand(id), cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}