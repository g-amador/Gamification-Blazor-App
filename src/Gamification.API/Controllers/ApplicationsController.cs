using Gamification.Application.DTOs;
using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.API.Controllers;

/// <summary>
/// API endpoints for managing applications.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationsController"/> class.
    /// </summary>
    public ApplicationsController(IApplicationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves an application by its identifier.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Creates a new application.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateApplicationDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Created(result.Url, result);
    }

    /// <summary>
    /// Updates an existing application.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateApplicationDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes an application by its identifier.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
