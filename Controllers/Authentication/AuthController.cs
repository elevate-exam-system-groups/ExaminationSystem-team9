using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Authentication;
using ExaminationSystem.Domain.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers.Authentication;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
