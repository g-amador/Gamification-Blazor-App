namespace Gamification.Application.DTOs.Event;

/// <summary>
/// Represents detailed information about an event.
/// </summary>
public class EventDetailsDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
