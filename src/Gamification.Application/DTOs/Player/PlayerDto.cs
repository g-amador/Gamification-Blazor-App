using Gamification.Application.DTOs.Badge;

namespace Gamification.Application.DTOs.Player;

/// <summary>
/// Represents detailed information about a player.
/// </summary>
public class PlayerDto
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
    /// Badges earned by the player.
    /// </summary>
    public List<BadgeDto> Badges { get; set; } = new();
}
