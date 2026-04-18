using ExaminationSystem.Abstractions;
using ExaminationSystem.Abstractions.Constants;
using ExaminationSystem.Domain.DTOs.Authentication;
using ExaminationSystem.Domain.Entities.Authentication;
using ExaminationSystem.Domain.Interfaces.Authentication;
using ExaminationSystem.Errors.Authentication;
using ExaminationSystem.Infrastructure.Persistence;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Implementations.Authentication;

public class AuthService(UserManager<ApplicationUser> userManager, IEmailSender emailSender,
    ApplicationDbContext context)
    : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<Guid>> RegisterAsync(RegisterRequest Request, CancellationToken cancellationToken = default)
    {
        var EmailIsExisting = await _userManager.Users.AnyAsync(c => c.Email == Request.Email, cancellationToken);

        if (EmailIsExisting)
            return Result.Failure<Guid>(UserError.DuplicatedEmail);

        var user = Request.Adapt<ApplicationUser>();

        var result = await _userManager.CreateAsync(user, Request.Password);
        await _userManager.AddToRoleAsync(user, DefaultRoles.Student);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();

            return Result.Failure<Guid>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var code = GenerateOtp();

        var body = $"Use this Otp to Confirm your email. otp is =>  {code} 'it will Expires After 10 Minutes' ";

        await _emailSender.SendEmailAsync(user.Email!, "Confirmation Your Email", body);

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

    private static string GenerateOtp() => Random.Shared.Next(100000, 999999).ToString();
}
