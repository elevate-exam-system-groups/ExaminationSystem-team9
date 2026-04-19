using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities.Authentication;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ExaminationSystem.Features.Authentication.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(
    UserManager<ApplicationUser> userManager,
    IEmailSender emailSender,
    ApplicationDbContext context)
    : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || user.IsDeleted)
            return Result.Success();

        var rawToken = TokenHelper.GenerateSecureToken();
        var tokenHash = TokenHelper.HashToken(rawToken);

        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.Now.AddMinutes(15),
            IsUsed = false,
            CreatedAt = DateTime.Now
        };

        await _context.PasswordResetTokens.AddAsync(resetToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var body = $"Use the following token to reset your password. It expires in 15 minutes.\n\n" +
                   $"Token: {rawToken}\n\n" +
                   $"If you did not request a password reset, please ignore this email.";

        await _emailSender.SendEmailAsync(user.Email!, "Password Reset Request", body);

        return Result.Success();
    }
}
