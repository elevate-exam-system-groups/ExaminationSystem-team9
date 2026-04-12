using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<OtpCode> OtpCodes { get; set; }
    public DbSet<Diploma> Diplomas { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<QuizAttempt> QuizAttempts { get; set; }
    public DbSet<AttemptAnswer> AttemptAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Diploma>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Enrollment>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Quiz>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Question>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Option>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<QuizAttempt>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<AttemptAnswer>().HasQueryFilter(e => !e.IsDeleted);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        var cascadeFks = builder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetForeignKeys())
                    .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

        foreach (var fk in cascadeFks)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(builder);
    }
}
