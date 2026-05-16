using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;
using Gamification.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure.Repositories;

/// <inheritdoc />
public class BadgeRepository : IBadgeRepository
{
    private readonly GamificationDbContext _db;

    public BadgeRepository(GamificationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<List<BadgeEntity>> GetAllAsync(int applicationId)
    {
        // Return all badges for the application
        return _db.Badges
            .Where(b => b.ApplicationId == applicationId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public Task<BadgeEntity?> GetByIdAsync(int applicationId, int badgeId)
    {
        // Return badge by id
        return _db.Badges
            .FirstOrDefaultAsync(b => b.ApplicationId == applicationId && b.Id == badgeId);
    }

    /// <inheritdoc />
    public async Task AddAsync(BadgeEntity entity)
    {
        // Add new badge
        await _db.Badges.AddAsync(entity);
    }

    /// <inheritdoc />
    public Task UpdateAsync(BadgeEntity entity)
    {
        // Mark as modified
        _db.Badges.Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(BadgeEntity entity)
    {
        // Remove badge
        _db.Badges.Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SaveChangesAsync()
    {
        // Persist changes
        return _db.SaveChangesAsync();
    }
}
