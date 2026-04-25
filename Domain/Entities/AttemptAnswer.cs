/* File Overview
 * File: AttemptAnswer.cs
 * Purpose: Domain model: core business entities and relationships used across the application.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

namespace ExaminationSystem.Domain.Entities;

public class AttemptAnswer : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid QuizAttemptId { get; set; }  
    public Guid QuestionId { get; set; }
    public Guid? SelectedOptionId { get; set; }
    public bool? IsCorrect { get; set; }
    public DateTime AnsweredAt { get; set; }
    public QuizAttempt QuizAttempt { get; set; } = default!;
    public Question Question { get; set; } = default!;
    public Option? SelectedOption { get; set; }
}

