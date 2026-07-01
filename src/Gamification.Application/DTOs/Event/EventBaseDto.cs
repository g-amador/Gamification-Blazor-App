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
    /// Type of the event.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of the event.
    /// </summary>
    public DateTime Timestamp { get; set; }
}
