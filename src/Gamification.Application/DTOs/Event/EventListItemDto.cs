namespace Gamification.Application.DTOs.Event;

/// <summary>
/// Represents an event in list responses.
/// </summary>
public class EventListItemDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
