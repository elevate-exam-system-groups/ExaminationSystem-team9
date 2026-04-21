namespace ExaminationSystem.DTOs.Authentication;

public record AuthResponse(
    Guid Id,
    string Email,
    string FullName,
    IList<string> Roles,
    string Token,
    int ExpiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);
