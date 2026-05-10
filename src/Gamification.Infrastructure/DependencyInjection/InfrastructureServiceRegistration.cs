using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Infrastructure.Data;
using Gamification.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gamification.Infrastructure.DependencyInjection;

/// <summary>
/// Registers infrastructure services.
/// </summary>
public static class InfrastructureServiceRegistration
{
    /// <summary>
    /// Adds infrastructure services to the DI container.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString, bool useInMemory)
    {
        if (useInMemory)
        {
            // Use in-memory database for development/testing
            services.AddDbContext<GamificationDbContext>(options =>
                options.UseInMemoryDatabase("GamificationDb"));
        }
        else
        {
            // Use SQL Server in production
            services.AddDbContext<GamificationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        // Register repository + service
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IApplicationService, ApplicationService>();

        return services;
    }
}
