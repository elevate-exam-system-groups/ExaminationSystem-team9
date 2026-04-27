using ExaminationSystem.Abstractions.Constants;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.DTOs.Authentication;
using System.Security.Cryptography;

namespace ExaminationSystem.Infrastructure.Implementations.Authentication;

public class AuthService(UserManager<ApplicationUser> userManager, IEmailSender emailSender,
    ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IJwtProvider jwtProvider,
    ILogger<AuthService> logger)
    : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ApplicationDbContext _context = context;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ILogger<AuthService> _logger = logger;
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

        var hashCode = BCrypt.Net.BCrypt.HashPassword(code);

        var otpCode = new OtpCode
        {
            UserId = user.Id,
            Email = user.Email!,
            Code = hashCode,
            CreatedAt = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddMinutes(10)
        };

        await _context.OtpCodes.AddAsync(otpCode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await SendConfirmationEmailAsync(user, code);

        return Result.Success(user.Id);
    }

    public async Task<Result> ConfirmationEmailAsync(ConfirmationEmailRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result.Failure(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailIsConfirmed);

        var otpCode = await _context.OtpCodes
            .Where(c => c.Email == request.Email && c.UserId == user.Id && !c.IsUsed)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (otpCode is null)
            return Result.Failure(UserErrors.InvalidCode);

        if (otpCode.AttemptCount >= 5)
        {
            otpCode.IsUsed = true;

            _context.OtpCodes.Update(otpCode);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Failure(UserErrors.EndOfAttempt);
        }

        var otpVerify = BCrypt.Net.BCrypt.Verify(request.OtpCode, otpCode.Code);

        if (!otpVerify)
        {
            otpCode.AttemptCount = otpCode.AttemptCount + 1;

            _context.OtpCodes.Update(otpCode);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Failure(UserErrors.InvalidCode);
        }

        if (otpCode.ExpirationDate < DateTime.UtcNow)
            return Result.Failure(UserErrors.OtpIsExpire);

        user.EmailConfirmed = true;
        user.AccountStatus = AccountStatus.Active;
        await _userManager.UpdateAsync(user);

        await _userManager.AddToRoleAsync(user, DefaultRoles.Student);

        otpCode.IsUsed = true;

        _context.OtpCodes.Update(otpCode);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Success();   // More secure

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailIsConfirmed);

        var oldOtpCode = await _context.OtpCodes
            .FirstOrDefaultAsync(c => c.Email == email && c.UserId == user.Id && !c.IsUsed, cancellationToken);

        if (oldOtpCode is not null)
        {
            oldOtpCode.IsUsed = true;
            _context.UpdateRange(oldOtpCode);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var code = GenerateOtp();

        var hashCode = BCrypt.Net.BCrypt.HashPassword(code);

        var otpCode = new OtpCode
        {
            UserId = user.Id,
            Email = user.Email!,
            Code = hashCode,
            CreatedAt = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddMinutes(10)
        };

        await _context.OtpCodes.AddAsync(otpCode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await SendConfirmationEmailAsync(user, code);

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

    private async Task SendConfirmationEmailAsync(ApplicationUser user, string code)
    {
        var body = $"Use this Otp to Confirm your email. otp is =>  {code}  \n 'it will Expires After 10 Minutes' ";

        await _emailSender.SendEmailAsync(user.Email!, "Confirmation Your Email", body);

        _logger.LogInformation("Otp code => {otp}", code);
    }

    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private static string GenerateOtp() => Random.Shared.Next(100000, 999999).ToString();
}
