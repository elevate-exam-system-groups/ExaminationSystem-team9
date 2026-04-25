/* File Overview
 * File: QuizAttemptConfiguration.cs
 * Purpose: EF Core model configuration: fluent mapping, indexes, and relational constraints for persistence.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.EntitiesConfigurations;

public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.HasOne(x => x.Result)
            .WithOne(x => x.QuizAttempt)
            .HasForeignKey<QuizAttemptResult>(x => x.QuizAttemptId);
    }
}

