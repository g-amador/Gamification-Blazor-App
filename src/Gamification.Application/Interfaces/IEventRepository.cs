using Gamification.Domain.Entities;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Provides persistence operations for events.
/// </summary>
public interface IEventRepository
{
    Task<List<EventEntity>> GetAllAsync(int applicationId);
    Task<EventEntity?> GetByIdAsync(int applicationId, int eventId);
    Task AddAsync(EventEntity entity);
    Task SaveChangesAsync();
}
