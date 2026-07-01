namespace Gamification.Domain.Entities;

/// <summary>
/// Represents a player belonging to an application.
/// </summary>
public class PlayerEntity
{
    /// <summary>
    /// Unique identifier of the player.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// First name of the player.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name of the player.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Email of the player.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Total number of points earned by the player.
    /// </summary>
    public int NumberOfPoints { get; set; }

    /// <summary>
    /// Application the player belongs to.
    /// </summary>
    public int ApplicationId { get; set; }
    public ApplicationEntity Application { get; set; } = null!;

    /// <summary>
    /// Badges earned by the player.
    /// </summary>
    public List<BadgeEntity> Badges { get; set; } = [];
}
