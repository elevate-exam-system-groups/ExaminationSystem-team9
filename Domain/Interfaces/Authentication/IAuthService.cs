using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Authentication;

namespace ExaminationSystem.Domain.Interfaces.Authentication;

public interface IAuthService
{
    Task<Result<Guid>> RegisterAsync(RegisterRequest Request, CancellationToken cancellationToken = default);
}
