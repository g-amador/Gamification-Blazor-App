namespace Gamification.Application.DTOs.EventType;

/// <summary>
/// Represents the response returned when an event type is created.
/// </summary>
public class EventTypeCreatedResponseDto
{
    /// <summary>
    /// Status of the creation operation.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Relative URL of the newly created event type.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier of the newly created event type.
    /// </summary>
    public int Id { get; set; }
}
