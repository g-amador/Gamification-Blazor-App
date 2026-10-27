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

    /// <summary>
    /// Event types registered in the system.
    /// </summary>
    public DbSet<EventTypeEntity> EventTypes => Set<EventTypeEntity>();

    /// <summary>
    /// Badge categories registered in the system.
    /// </summary>
    public DbSet<BadgeCategoryEntity> BadgeCategories => Set<BadgeCategoryEntity>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // SQL Server accepts only a single cascading path per table.
        // Applications cascade to Players (which cascade to their Events) and to
        // Rules directly; every other reference is protected with NO ACTION.
        base.OnModelCreating(modelBuilder);

        // Events are deleted through their player.
        modelBuilder.Entity<EventEntity>()
            .HasOne(e => e.Application)
            .WithMany()
            .HasForeignKey(e => e.ApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventEntity>()
            .HasOne(e => e.EventType)
            .WithMany()
            .HasForeignKey(e => e.EventTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Rules are deleted with their application.
        modelBuilder.Entity<RuleEntity>()
            .HasOne(r => r.EventType)
            .WithMany()
            .HasForeignKey(r => r.EventTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Optional references must not create additional cascade paths.
        modelBuilder.Entity<RuleEntity>()
            .HasOne(r => r.Badge)
            .WithMany()
            .HasForeignKey(r => r.BadgeId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<BadgeEntity>()
            .HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
