using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;
using Gamification.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure.Repositories;

/// <inheritdoc />
public class EventTypeRepository : IEventTypeRepository
{
    private readonly GamificationDbContext _db;

    public EventTypeRepository(GamificationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<List<EventTypeEntity>> GetAllAsync(int applicationId)
    {
        // Return all event types for the application
        return _db.EventTypes
            .Where(t => t.ApplicationId == applicationId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public Task<EventTypeEntity?> GetByIdAsync(int applicationId, int eventTypeId)
    {
        // Return event type by id
        return _db.EventTypes
            .FirstOrDefaultAsync(t => t.ApplicationId == applicationId && t.Id == eventTypeId);
    }

    /// <inheritdoc />
    public async Task AddAsync(EventTypeEntity entity)
    {
        // Add new event type to EF Core tracking
        await _db.EventTypes.AddAsync(entity);
    }

    /// <inheritdoc />
    public Task UpdateAsync(EventTypeEntity entity)
    {
        // Mark entity as modified
        _db.EventTypes.Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SaveChangesAsync()
    {
        // Persist changes to database
        return _db.SaveChangesAsync();
    }

    /// <inheritdoc />
    public Task<bool> NameExistsAsync(int applicationId, string name)
    {
        // Check if any event type of the application uses this name
        return _db.EventTypes
            .AnyAsync(t => t.ApplicationId == applicationId && t.Name == name);
    }
}
