using Gamification.Application.DTOs.Leaderboard;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for retrieving the leaderboard.
/// </summary>
public interface ILeaderboardService
{
    /// <summary>
    /// Retrieves the top 5 players of an application ordered by points.
    /// </summary>
    Task<LeaderboardResponseDto?> GetAsync(string apiKey, string apiPassword);
}
