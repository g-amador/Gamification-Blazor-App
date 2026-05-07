using Gamification.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure.Data;

/// <summary>
/// Database context for the gamification system.
/// </summary>
public class GamificationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GamificationDbContext"/> class.
    /// </summary>
    public GamificationDbContext(DbContextOptions<GamificationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Applications registered in the system.
    /// </summary>
    public DbSet<ApplicationEntity> Applications => Set<ApplicationEntity>();
}
