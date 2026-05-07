using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Infrastructure.Data;
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
            services.AddDbContext<GamificationDbContext>(options =>
                options.UseInMemoryDatabase("GamificationDb"));
        }
        else
        {
            services.AddDbContext<GamificationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        services.AddScoped<IApplicationService, ApplicationService>();

        return services;
    }
}
