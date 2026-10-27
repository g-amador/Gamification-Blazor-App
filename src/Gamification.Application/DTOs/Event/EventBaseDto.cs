namespace Gamification.Application.DTOs.Event;

/// <summary>
/// Base class for event creation.
/// </summary>
public class EventBaseDto
{
    /// <summary>
    /// Player associated with the event.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Identifier of the event type of the event (defined with the event types resource).
    /// </summary>
    public int EventTypeId { get; set; }

    /// <summary>
    /// Timestamp of the event.
    /// </summary>
    public DateTime Timestamp { get; set; }
}
