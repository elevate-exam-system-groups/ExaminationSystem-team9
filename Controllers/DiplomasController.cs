using ExaminationSystem.Domain.DTOs.Common;
using ExaminationSystem.Features.Diplomas.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiplomasController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllDiplomaQuery(filters), cancellationToken);

            return Ok(result);
        }
    }
}
