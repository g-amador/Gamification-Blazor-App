using Gamification.Domain.Entities;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Provides persistence operations for badges.
/// </summary>
public interface IBadgeRepository
{
    /// <summary>
    /// Retrieves all badges of an application.
    /// </summary>
    Task<List<BadgeEntity>> GetAllAsync(int applicationId);

    /// <summary>
    /// Retrieves a badge by id within an application.
    /// </summary>
    Task<BadgeEntity?> GetByIdAsync(int applicationId, int badgeId);

    /// <summary>
    /// Adds a new badge.
    /// </summary>
    Task AddAsync(BadgeEntity entity);

    /// <summary>
    /// Updates an existing badge.
    /// </summary>
    Task UpdateAsync(BadgeEntity entity);

    /// <summary>
    /// Deletes a badge.
    /// </summary>
    Task DeleteAsync(BadgeEntity entity);

    /// <summary>
    /// Saves changes to the data store.
    /// </summary>
    Task SaveChangesAsync();
}
