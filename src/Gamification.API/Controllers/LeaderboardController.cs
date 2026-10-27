using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>
/// Returns the leaderboard of an application.
/// All operations related to the leaderboard resource are in this section.
/// The leaderboard returns the list of the best players of an application.
/// </summary>
[ApiController]
[Route("leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _service;

    public LeaderboardController(ILeaderboardService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns the leaderboard of the application:
    /// the list of the five best players of an application ordered by points.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var leaderboard = await _service.GetAsync(apiKey, apiPassword);
        return Ok(leaderboard);
    }
}
