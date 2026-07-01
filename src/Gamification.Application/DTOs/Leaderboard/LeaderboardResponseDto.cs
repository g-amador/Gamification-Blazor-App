namespace Gamification.Application.DTOs.Leaderboard;

/// <summary>
/// Represents the leaderboard of an application.
/// </summary>
public class LeaderboardResponseDto
{
    /// <summary>
    /// Name of the application.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the application.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ranking of the top players.
    /// </summary>
    public List<LeaderboardItemDto> Ranking { get; set; } = [];
}
