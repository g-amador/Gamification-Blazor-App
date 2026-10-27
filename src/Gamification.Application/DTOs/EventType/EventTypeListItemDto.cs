namespace Gamification.Application.DTOs.EventType;

/// <summary>
/// Represents an event type in list responses.
/// </summary>
public class EventTypeListItemDto
{
    /// <summary>
    /// Unique identifier of the event type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the event type.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
