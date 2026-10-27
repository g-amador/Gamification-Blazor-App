namespace Gamification.Application.DTOs.EventType;

/// <summary>
/// Represents the detailed information of an event type.
/// </summary>
public class EventTypeDetailsDto
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
