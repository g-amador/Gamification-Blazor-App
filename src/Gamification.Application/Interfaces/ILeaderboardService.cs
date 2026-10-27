using Gamification.Application.DTOs.Leaderboard;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for retrieving the leaderboard.
/// All operations related to the leaderboard resource are in this section.
/// The leaderboard returns the list of the best players of an application.
/// </summary>
public interface ILeaderboardService
{
    /// <summary>
    /// Retrieves the leaderboard of the application:
    /// the top 5 players of an application ordered by points.
    /// </summary>
    Task<LeaderboardResponseDto> GetAsync(string apiKey, string apiPassword);
}
