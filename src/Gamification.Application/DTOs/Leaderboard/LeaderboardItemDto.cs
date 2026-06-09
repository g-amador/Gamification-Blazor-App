namespace Gamification.Application.DTOs.Leaderboard;

/// <summary>
/// Represents a player entry in the leaderboard.
/// </summary>
public class LeaderboardItemDto
{
    /// <summary>
    /// Unique identifier of the player.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Total number of points earned by the player.
    /// </summary>
    public int Points { get; set; }
}
