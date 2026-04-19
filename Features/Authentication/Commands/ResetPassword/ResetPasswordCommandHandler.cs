using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities.Authentication;
using ExaminationSystem.Errors.Authentication;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandHandler(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context)
    : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = TokenHelper.HashToken(request.Token);

        var resetToken = await _context.PasswordResetTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash && !t.IsUsed && t.ExpiresAt > DateTime.Now,cancellationToken);

        if (resetToken is null)
            return Result.Failure(UserError.InvalidOrExpiredResetToken);

        var user = resetToken.User;

        var removeResult = await _userManager.RemovePasswordAsync(user);
        if (!removeResult.Succeeded)
        {
            var error = removeResult.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var addResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
        if (!addResult.Succeeded)
        {
            var error = addResult.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        resetToken.IsUsed = true;

        await _userManager.UpdateSecurityStampAsync(user);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
