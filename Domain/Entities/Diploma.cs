/* File Overview
 * File: Diploma.cs
 * Purpose: Domain model: core business entities and relationships used across the application.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities;

public class Diploma : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DiplomaStatus Status { get; set; } = DiplomaStatus.Draft;
    public DateTime? DeletedAt { get; set; }
    public ICollection<Quiz> Quizzes { get; set; } = [];
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}

