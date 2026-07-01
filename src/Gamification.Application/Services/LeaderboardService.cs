using Gamification.Application.DTOs.Leaderboard;
using Gamification.Application.Interfaces;

namespace Gamification.Application.Services;

/// <inheritdoc />
public class LeaderboardService : ILeaderboardService
{
    private readonly IApplicationRepository _appRepo;
    private readonly IPlayerRepository _playerRepo;

    public LeaderboardService(IApplicationRepository appRepo, IPlayerRepository playerRepo)
    {
        _appRepo = appRepo;
        _playerRepo = playerRepo;
    }

    /// <inheritdoc />
    public async Task<LeaderboardResponseDto?> GetAsync(string apiKey, string apiPassword)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword);
        if (app is null)
        {
            return null;
        }

        // Load all players
        var players = await _playerRepo.GetAllAsync(app.Id);

        // Order by points and take top 5
        var ranking = players
            .OrderByDescending(p => p.NumberOfPoints)
            .Take(5)
            .Select(p => new LeaderboardItemDto
            {
                PlayerId = p.Id,
                Points = p.NumberOfPoints
            })
            .ToList();

        // Build response
        return new LeaderboardResponseDto
        {
            Name = app.Name,
            Description = app.Description,
            Ranking = ranking
        };
    }
}
