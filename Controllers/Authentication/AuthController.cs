using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Authentication;
using ExaminationSystem.Domain.Interfaces.Authentication;
using ExaminationSystem.Features.Authentication.Commands.ForgotPassword;
using ExaminationSystem.Features.Authentication.Commands.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers.Authentication;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService _authService, IMediator _mediator) : ControllerBase
{

    /// <summary>POST /api/auth/register</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
