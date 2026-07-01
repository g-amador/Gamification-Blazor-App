using Gamification.Application.DTOs.Rule;
using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>
/// Manages rules of an application.
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
    /// Creates a new rule.
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
    /// Updates a rule.
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
    /// Deletes a rule.
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
