using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>
/// Returns the leaderboard of an application.
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
    /// Returns the list of the five best players of an application.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var leaderboard = await _service.GetAsync(apiKey, apiPassword);
        if (leaderboard is null)
        {
            return NotFound();
        }

        return Ok(leaderboard);
    }
}
