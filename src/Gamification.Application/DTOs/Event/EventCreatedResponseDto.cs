namespace Gamification.Application.DTOs.Event;

/// <summary>
/// Represents the response returned after creating an event.
/// </summary>
public class EventCreatedResponseDto
{
    public string Status { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int Id { get; set; }
}
