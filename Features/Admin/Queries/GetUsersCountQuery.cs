using ExaminationSystem.Abstractions.Constants;
using ExaminationSystem.Domain.Entities.Authentication;
using ExaminationSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Admin.Queries;

public record GetUsersCountQuery() : IRequest<int>;

public class GetUsersCountQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetUsersCountQuery, int>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    public async Task<int> Handle(GetUsersCountQuery request, CancellationToken cancellationToken) =>
        await _userManager.Users
            .Where(c => !c.IsDeleted
            && c.AccountStatus == AccountStatus.Active
            && c.EmailConfirmed
            && c.Id != DefaultUsers.AdminId)
            .CountAsync(cancellationToken);

}