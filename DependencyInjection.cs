using ExaminationSystem.Domain.Entities.Authentication;
using ExaminationSystem.Domain.Interfaces.Authentication;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.Infrastructure.Implementations;
using ExaminationSystem.Infrastructure.Implementations.Authentication;
using ExaminationSystem.Infrastructure.Implementations.Repositories;
using ExaminationSystem.Infrastructure.Persistence;
using ExaminationSystem.Settings;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

namespace ExaminationSystem;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
           throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );


        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });

        services
            .AddAuthenticationConfig()
            .AddFluentValidationConfig()
            .AddMapsterConfig();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }

    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        //add mapster
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }
    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
    private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

        return services;
    }
}
