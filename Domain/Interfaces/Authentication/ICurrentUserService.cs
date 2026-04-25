/* File Overview
 * File: ICurrentUserService.cs
 * Purpose: Domain abstractions: interfaces/contracts that decouple application logic from infrastructure details.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

namespace ExaminationSystem.Domain.Interfaces.Authentication;

public interface ICurrentUserService
{
    Guid? GetCurrentUserId();
}

