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

    /// <summary>
    /// Players registered in the system.
    /// </summary>
    public DbSet<PlayerEntity> Players => Set<PlayerEntity>();

    /// <summary>
    /// Badges registered in the system.
    /// </summary>
    public DbSet<BadgeEntity> Badges => Set<BadgeEntity>();

    /// <summary>
    /// Rules registered in the system.
    /// </summary>
    public DbSet<RuleEntity> Rules => Set<RuleEntity>();
}
