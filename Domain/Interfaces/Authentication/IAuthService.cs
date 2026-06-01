using ExaminationSystem.DTOs.Authentication;

namespace ExaminationSystem.Domain.Interfaces.Authentication;

public interface IAuthService
{
    Task<Result<Guid>> RegisterAsync(RegisterRequest Request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result> ConfirmationEmailAsync(ConfirmationEmailRequest request, CancellationToken cancellationToken = default);
    Task<Result> ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken = default);
}
