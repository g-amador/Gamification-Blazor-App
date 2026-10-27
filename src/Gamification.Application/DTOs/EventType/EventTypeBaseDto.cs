namespace Gamification.Application.DTOs.EventType;

/// <summary>
/// Base class for event type creation and update operations.
/// </summary>
public class EventTypeBaseDto
{
    /// <summary>
    /// Name of the event type.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
