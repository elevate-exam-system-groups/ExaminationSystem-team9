using ExaminationSystem.DTOs.Authentication;
using ExaminationSystem.Features.Authentication.Commands.ForgotPassword;
using ExaminationSystem.Features.Authentication.Commands.ResetPassword;

namespace ExaminationSystem.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService _authService, IMediator _mediator) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

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

    [HttpPost("email-confirm")]
    public async Task<IActionResult> ConfirmationEmail([FromBody] ConfirmationEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ConfirmationEmailAsync(request, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("resend-email-confirm")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request.Email, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
