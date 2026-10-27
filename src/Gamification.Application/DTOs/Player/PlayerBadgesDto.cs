using Gamification.Application.DTOs.Badge;

namespace Gamification.Application.DTOs.Player;

/// <summary>
/// Represents the list of badges owned by a player.
/// </summary>
public class PlayerBadgesDto
{
    /// <summary>
    /// Unique identifier of the player.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Badges owned by the player.
    /// </summary>
    public List<BadgeDto> Badges { get; set; } = [];
}
