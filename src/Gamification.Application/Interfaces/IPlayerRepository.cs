using Gamification.Domain.Entities;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Provides persistence operations for players.
/// </summary>
public interface IPlayerRepository
{
    /// <summary>
    /// Retrieves all players belonging to an application.
    /// </summary>
    Task<List<PlayerEntity>> GetAllAsync(int applicationId);

    /// <summary>
    /// Retrieves a player by id within an application.
    /// </summary>
    Task<PlayerEntity?> GetByIdAsync(int applicationId, int playerId);

    /// <summary>
    /// Adds a new player.
    /// </summary>
    Task AddAsync(PlayerEntity entity);

    /// <summary>
    /// Updates an existing player.
    /// </summary>
    Task UpdateAsync(PlayerEntity entity);

    /// <summary>
    /// Deletes a player.
    /// </summary>
    Task DeleteAsync(PlayerEntity entity);

    /// <summary>
    /// Saves changes to the data store.
    /// </summary>
    Task SaveChangesAsync();
}
