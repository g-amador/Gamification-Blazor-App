using Gamification.Application.DTOs.EventType;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing event types.
/// All operations related to the event type resource are in this section.
/// All events that are recorded by the gamification API are associated with a type.
/// The types can be defined by the application using the API with the related API calls.
/// Then event types can be used in rules to define the conditions needed to obtain a badge.
/// </summary>
public interface IEventTypeService
{
    /// <summary>
    /// Retrieves the list of all event types defined for the application.
    /// </summary>
    Task<List<EventTypeListItemDto>> GetAllAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Retrieves information about a specific event type id.
    /// </summary>
    Task<EventTypeDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int eventTypeId);

    /// <summary>
    /// Adds a new event type in the application.
    /// </summary>
    Task<EventTypeCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateEventTypeDto dto);

    /// <summary>
    /// Updates the information of the given event type.
    /// </summary>
    Task<bool> UpdateAsync(string apiKey, string apiPassword, int eventTypeId, UpdateEventTypeDto dto);
}
