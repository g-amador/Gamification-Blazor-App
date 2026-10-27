using Gamification.Application.DTOs.Event;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing events.
/// All operations related to the event resource are in this section.
/// Everything that has to be monitored by the gamification platform is recorded
/// as an event and has to be posted to the gamification API.
/// When something happens to a player or when a player does something, an event
/// should be created. An event is linked to a player and is of a type.
/// Multiple events of the same type and player can be created.
/// An event cannot be updated or deleted.
/// </summary>
public interface IEventService
{
    /// <summary>
    /// Retrieves the list of all events registered on the gamification server for the application.
    /// </summary>
    Task<List<EventListItemDto>> GetAllAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Retrieves detailed information about a specific event identified by its id.
    /// </summary>
    Task<EventDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int eventId);

    /// <summary>
    /// Registers a new event on the gamification platform for the application.
    /// When an event is created, the rules are browsed and the corresponding
    /// points and badges are given to the player.
    /// If a badge is already won by the player, nothing happens. Returns the event's id.
    /// </summary>
    Task<EventCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateEventDto dto);
}
