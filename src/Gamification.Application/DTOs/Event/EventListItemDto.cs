namespace Gamification.Application.DTOs.Event;

/// <summary>
/// Represents an event in list responses.
/// </summary>
public class EventListItemDto
{
    public int Id { get; set; }

    /// <summary>
    /// Player associated with the event.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Identifier of the event type of the event.
    /// </summary>
    public int EventTypeId { get; set; }

    /// <summary>
    /// Name of the event type of the event (read convenience only).
    /// </summary>
    public string EventTypeName { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }
}
