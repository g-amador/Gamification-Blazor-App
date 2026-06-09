namespace Gamification.Domain.Entities;

/// <summary>
/// Represents an event triggered by a player.
/// </summary>
public class EventEntity
{
    /// <summary>
    /// Unique identifier of the event.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Player associated with the event.
    /// </summary>
    public int PlayerId { get; set; }

    public PlayerEntity Player { get; set; } = null!;

    /// <summary>
    /// Type of the event.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of the event.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Application the event belongs to.
    /// </summary>
    public int ApplicationId { get; set; }
    public ApplicationEntity Application { get; set; } = null!;
}
