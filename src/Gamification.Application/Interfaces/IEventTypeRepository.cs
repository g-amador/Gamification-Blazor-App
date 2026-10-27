using Gamification.Domain.Entities;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Provides persistence operations for event types.
/// All operations related to the event type resource are in this section.
/// All events that are recorded by the gamification API are associated with a type.
/// The types can be defined by the application using the API with the related API calls.
/// Then event types can be used in rules to define the conditions needed to obtain a badge.
/// </summary>
public interface IEventTypeRepository
{
    /// <summary>
    /// Retrieves all event types belonging to an application.
    /// </summary>
    Task<List<EventTypeEntity>> GetAllAsync(int applicationId);

    /// <summary>
    /// Retrieves an event type by id within an application.
    /// </summary>
    Task<EventTypeEntity?> GetByIdAsync(int applicationId, int eventTypeId);

    /// <summary>
    /// Adds a new event type.
    /// </summary>
    Task AddAsync(EventTypeEntity entity);

    /// <summary>
    /// Updates an existing event type.
    /// </summary>
    Task UpdateAsync(EventTypeEntity entity);

    /// <summary>
    /// Saves changes to the data store.
    /// </summary>
    Task SaveChangesAsync();

    /// <summary>
    /// Checks whether an event type with the given name already exists in the application.
    /// </summary>
    Task<bool> NameExistsAsync(int applicationId, string name);
}
