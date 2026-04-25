/* File Overview
 * File: CurrentUserService.cs
 * Purpose: Infrastructure services: concrete implementations for domain/application abstractions.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ExaminationSystem.Infrastructure.Implementations.Authentication;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var claimValue =
            user?.FindFirstValue(ClaimTypes.NameIdentifier) ??
            user?.FindFirstValue("sub");

        if (Guid.TryParse(claimValue, out var claimId))
        {
            return claimId;
        }

        var headerValue = _httpContextAccessor.HttpContext?.Request.Headers["X-Student-Id"].FirstOrDefault();
        if (Guid.TryParse(headerValue, out var headerId))
        {
            return headerId;
        }

        return null;
    }
}

