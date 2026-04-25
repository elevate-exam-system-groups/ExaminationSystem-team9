/* File Overview
 * File: QuizAttemptResult.cs
 * Purpose: Domain model: core business entities and relationships used across the application.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

namespace ExaminationSystem.Domain.Entities;

public class QuizAttemptResult : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid QuizAttemptId { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public decimal Score { get; set; }
    public bool Passed { get; set; }
    public DateTime FinalizedAt { get; set; } = DateTime.UtcNow;
    public QuizAttempt QuizAttempt { get; set; } = default!;
}

