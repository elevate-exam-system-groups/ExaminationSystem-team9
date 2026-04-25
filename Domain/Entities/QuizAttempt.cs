/* File Overview
 * File: QuizAttempt.cs
 * Purpose: Domain model: core business entities and relationships used across the application.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.Entities.Authentication;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities;

public class QuizAttempt : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Guid QuizId { get; set; }
    public QuizAttemptStatus Status { get; set; } = QuizAttemptStatus.InProgress;
    public DateTime StartTime { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public decimal? Score { get; set; }
    public bool? Passed { get; set; }

    public ApplicationUser Student { get; set; } = default!; 
    public Quiz Quiz { get; set; } = default!;
    public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();
    public QuizAttemptResult? Result { get; set; }
}

