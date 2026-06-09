using Gamification.Application.DTOs.Event;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing events.
/// </summary>
public interface IEventService
{
    Task<List<EventListItemDto>> GetAllAsync(string apiKey, string apiPassword);
    Task<EventDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int eventId);
    Task<EventCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateEventDto dto);
}
