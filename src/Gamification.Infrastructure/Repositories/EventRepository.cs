using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;
using Gamification.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure.Repositories;

/// <inheritdoc />
public class EventRepository : IEventRepository
{
    private readonly GamificationDbContext _db;

    public EventRepository(GamificationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<List<EventEntity>> GetAllAsync(int applicationId)
    {
        return _db.Events
            .Where(e => e.ApplicationId == applicationId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public Task<EventEntity?> GetByIdAsync(int applicationId, int eventId)
    {
        return _db.Events
            .FirstOrDefaultAsync(e => e.ApplicationId == applicationId && e.Id == eventId);
    }

    /// <inheritdoc />
    public async Task AddAsync(EventEntity entity)
    {
        await _db.Events.AddAsync(entity);
    }

    /// <inheritdoc />
    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}
