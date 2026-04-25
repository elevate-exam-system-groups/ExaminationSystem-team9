/* File Overview
 * File: MappingConfigurations.cs
 * Purpose: Object mapping configuration: defines transformation rules between entities and DTOs.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.DTOs.Authentication;
using ExaminationSystem.Domain.Entities.Authentication;
using Mapster;  

namespace ExaminationSystem.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //Auth Mapping
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => $"{src.Email}");
    }
}

