using Gamification.Application.DTOs.Rule;
using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>
/// Manages the rules of an application.
/// All operations related to the rule resource are in this section.
/// A rule defines when badges or points should be granted to a player.
/// Many rules can be created for the same event type.
/// </summary>
[ApiController]
[Route("rules")]
public class RulesController : ControllerBase
{
    private readonly IRuleService _service;

    public RulesController(IRuleService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns the list of all rules of an application.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var rules = await _service.GetAllAsync(apiKey, apiPassword);
        return Ok(new { items = rules });
    }

    /// <summary>
    /// Adds a new rule for the application. The badgeId is optional,
    /// the number of points is required but can be set to 0.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRuleDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var result = await _service.CreateAsync(apiKey, apiPassword, dto);
        return Created(result.Url, result);
    }

    /// <summary>
    /// Returns detailed information about a rule.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var rule = await _service.GetByIdAsync(apiKey, apiPassword, id);
        if (rule is null)
        {
            return NotFound();
        }

        return Ok(rule);
    }

    /// <summary>
    /// Updates a rule. This doesn't change badges and points given to players on past events.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRuleDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var success = await _service.UpdateAsync(apiKey, apiPassword, id, dto);
        return success ? Ok() : NotFound();
    }

    /// <summary>
    /// Deletes a rule. This doesn't remove badges and points given to players on past events.
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
