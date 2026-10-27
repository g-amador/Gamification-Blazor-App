using Gamification.Application.DTOs.Badge;
using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>
/// Manages the badges of an application.
/// All operations related to the badge resource are in this section.
/// A badge is a distinction, won by a player when he does something significant (an event).
/// </summary>
[ApiController]
[Route("badges")]
public class BadgesController : ControllerBase
{
    private readonly IBadgeService _service;

    public BadgesController(IBadgeService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns the list of all badges of an application.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var badges = await _service.GetAllAsync(apiKey, apiPassword);
        return Ok(new { items = badges });
    }

    /// <summary>
    /// Adds a new badge for the application. The categoryId is optional.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBadgeDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var result = await _service.CreateAsync(apiKey, apiPassword, dto);
        return Created(result.Url, result);
    }

    /// <summary>
    /// Returns information of the given badge, including categoryId when set.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var badge = await _service.GetByIdAsync(apiKey, apiPassword, id);
        if (badge is null)
        {
            return NotFound();
        }

        return Ok(badge);
    }

    /// <summary>
    /// Updates a badge. The categoryId is optional.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBadgeDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var success = await _service.UpdateAsync(apiKey, apiPassword, id, dto);
        return success ? Ok() : NotFound();
    }

    /// <summary>
    /// Deletes a badge. All links to this badge by rules are set to null.
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
