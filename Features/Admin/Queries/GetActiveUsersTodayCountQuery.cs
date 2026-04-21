using ExaminationSystem.Abstractions.Constants;
using ExaminationSystem.Domain.Entities.Authentication;
using ExaminationSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Admin.Queries;

public record GetActiveUsersTodayCountQuery() : IRequest<int>;

public class GetActiveUsersTodayCountQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetActiveUsersTodayCountQuery, int>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<int> Handle(GetActiveUsersTodayCountQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        return await _userManager.Users
            .Where(c => !c.IsDeleted
            && c.AccountStatus == AccountStatus.Active
            && c.EmailConfirmed
            && c.Id != DefaultUsers.AdminId
            && c.LastLoginAt.HasValue
            && c.LastLoginAt.Value.Date == today)
            .CountAsync(cancellationToken);
    }
}