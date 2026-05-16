using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;
using Gamification.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure.Repositories;

/// <inheritdoc />
public class PlayerRepository : IPlayerRepository
{
    private readonly GamificationDbContext _db;

    public PlayerRepository(GamificationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<List<PlayerEntity>> GetAllAsync(int applicationId)
    {
        // Return all players for the application
        return _db.Players
            .Where(p => p.ApplicationId == applicationId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public Task<PlayerEntity?> GetByIdAsync(int applicationId, int playerId)
    {
        // Return player with badges
        return _db.Players
            .Include(p => p.Badges)
            .FirstOrDefaultAsync(p => p.ApplicationId == applicationId && p.Id == playerId);
    }

    /// <inheritdoc />
    public async Task AddAsync(PlayerEntity entity)
    {
        // Add new player
        await _db.Players.AddAsync(entity);
    }

    /// <inheritdoc />
    public Task UpdateAsync(PlayerEntity entity)
    {
        // Mark as modified
        _db.Players.Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(PlayerEntity entity)
    {
        // Remove player
        _db.Players.Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SaveChangesAsync()
    {
        // Persist changes
        return _db.SaveChangesAsync();
    }
}
