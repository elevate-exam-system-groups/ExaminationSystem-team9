using ExaminationSystem.Abstractions.Constants;
using ExaminationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.EntitiesConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var passwordHasher = new PasswordHasher<ApplicationUser>();

        var adminUser = new ApplicationUser
        {
            Id = DefaultUsers.AdminId,
            FullName = DefaultUsers.AdminFullName,
            UserName = DefaultUsers.AdminEmail,
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            Email = DefaultUsers.AdminEmail,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword),
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            AccountStatus = AccountStatus.Active
        };

        builder.HasData(adminUser);

        builder.Property(c => c.FullName).HasMaxLength(200);
    }
}
