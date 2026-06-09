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
    /// Badges registered in the system.
    /// </summary>
    public DbSet<BadgeEntity> Badges => Set<BadgeEntity>();

    /// <summary>
    /// Events registered in the system.
    /// </summary>
    public DbSet<EventEntity> Events => Set<EventEntity>();

    /// <summary>
    /// Players registered in the system.
    /// </summary>
    public DbSet<PlayerEntity> Players => Set<PlayerEntity>();

    /// <summary>
    /// Rules registered in the system.
    /// </summary>
    public DbSet<RuleEntity> Rules => Set<RuleEntity>();
}
