/* File Overview
 * File: Question.cs
 * Purpose: Domain model: core business entities and relationships used across the application.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

namespace ExaminationSystem.Domain.Entities;

public class Question : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid QuizId { get; set; }
    public string Text { get; set; } = default!;
    public string? Explanation { get; set; }
    public int OrderIndex { get; set; } = 1;
    public DateTime? DeletedAt { get; set; }
    public Quiz Quiz { get; set; } = default!;
    public ICollection<Option> Options { get; set; } = [];
    public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = [];
}

