/* File Overview
 * File: Enrollment.cs
 * Purpose: Domain model: core business entities and relationships used across the application.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.Entities.Authentication;

namespace ExaminationSystem.Domain.Entities;

public class Enrollment : BaseEntity
{
    public int Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid DiplomaId { get; set; }
    public DateTime EnrolledAt { get; set; }
    public ApplicationUser Student { get; set; } = default!;
    public Diploma Diploma { get; set; } = default!;
}

