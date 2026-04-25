/* File Overview
 * File: QuizAttemptResultConfiguration.cs
 * Purpose: EF Core model configuration: fluent mapping, indexes, and relational constraints for persistence.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.EntitiesConfigurations;

public class QuizAttemptResultConfiguration : IEntityTypeConfiguration<QuizAttemptResult>
{
    public void Configure(EntityTypeBuilder<QuizAttemptResult> builder)
    {
        builder.HasIndex(x => x.QuizAttemptId).IsUnique();
        builder.Property(x => x.Score).HasColumnType("decimal(5,2)");
    }
}

