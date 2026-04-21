using ExaminationSystem.Abstractions.Constants;
using ExaminationSystem.DTOs.Authentication;
using System.Security.Cryptography;

namespace ExaminationSystem.Infrastructure.Implementations.Authentication;

public class AuthService(UserManager<ApplicationUser> userManager, IEmailSender emailSender,
    ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IJwtProvider jwtProvider)
    : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ApplicationDbContext _context = context;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpiryDays = 7;

    public async Task<Result<Guid>> RegisterAsync(RegisterRequest Request, CancellationToken cancellationToken = default)
    {
        var EmailIsExisting = await _userManager.Users.AnyAsync(c => c.Email == Request.Email, cancellationToken);

        if (EmailIsExisting)
            return Result.Failure<Guid>(UserErrors.DuplicatedEmail);

        var user = Request.Adapt<ApplicationUser>();

        var result = await _userManager.CreateAsync(user, Request.Password);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();

            return Result.Failure<Guid>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var code = GenerateOtp();

        var body = $"Use this Otp to Confirm your email. otp is =>  {code} 'it will Expires After 10 Minutes' ";

        await _emailSender.SendEmailAsync(user.Email!, "Confirmation Your Email", body);

        await _userManager.AddToRoleAsync(user, DefaultRoles.Student);    // move After login And confirm email

        var otpCode = new OtpCode
        {
            UserId = user.Id,
            Email = user.Email!,
            Code = code,
            CreatedAt = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false,
            AttemptCount = 0,
            ResendCount = 0
        };

        await _context.OtpCodes.AddAsync(otpCode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id);
    }

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        if (!result.Succeeded)
            return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);

        var userRoles = await _userManager.GetRolesAsync(user);

        var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRoles);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email!, user.FullName, Roles: userRoles, token, expiresIn, refreshToken, refreshTokenExpiration);

        return Result.Success(response);
    }

    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private static string GenerateOtp() => Random.Shared.Next(100000, 999999).ToString();
}
