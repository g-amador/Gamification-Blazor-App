using Gamification.Application.DTOs.Player;
using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>
/// Manages the players (users) of an application.
/// All operations related to the player resource are in this section.
/// A player is a user of the application; he can win points and badges when events are happening.
/// </summary>
[ApiController]
[Route("players")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _service;

    public PlayersController(IPlayerService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns the list of all players of an application.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var players = await _service.GetAllAsync(apiKey, apiPassword);
        return Ok(new { items = players });
    }

    /// <summary>
    /// Adds a new player.
    /// To manage points and badges, please go to the events resource.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlayerDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var result = await _service.CreateAsync(apiKey, apiPassword, dto);
        return Created(result.Url, result);
    }

    /// <summary>
    /// Returns detailed information about a player.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var player = await _service.GetByIdAsync(apiKey, apiPassword, id);
        if (player is null)
        {
            return NotFound();
        }

        return Ok(player);
    }

    /// <summary>
    /// Returns all badges of a specific player.
    /// </summary>
    [HttpGet("{id:int}/badges")]
    public async Task<IActionResult> GetBadges(int id)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var badges = await _service.GetBadgesAsync(apiKey, apiPassword, id);
        if (badges is null)
        {
            return NotFound();
        }

        return Ok(badges);
    }

    /// <summary>
    /// Updates the information of a player.
    /// The points and badges can only be updated by events.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePlayerDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var success = await _service.UpdateAsync(apiKey, apiPassword, id, dto);
        return success ? Ok() : NotFound();
    }

    /// <summary>
    /// Deletes a player. All events linked to the player are deleted in cascade.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var success = await _service.DeleteAsync(apiKey, apiPassword, id);
        return success ? Ok() : NotFound();
    }
}
