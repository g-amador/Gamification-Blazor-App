using Gamification.Application.DTOs.Application;
using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.API.Controllers;

/// <summary>
/// The application is the entry point of the API.
/// All other resources are linked to an application.
/// Each application has a unique apiKey and apiPassword that must be sent
/// in the HTTP headers for authentication.
/// </summary>
[ApiController]
[Route("application")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _service;

    public ApplicationsController(IApplicationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create a new application.
    /// This is the first step before using the API.
    /// Provide a name, description, apiKey and apiPassword.
    /// Returns the created application with its identifier.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateApplicationDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Created($"/application/{result.Id}", result);
    }

    /// <summary>
    /// Get an application.
    /// You must provide apiKey and apiPassword in the HTTP headers.
    /// Returns the name, description and key of the application.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!TryGetCredentials(out var apiKey, out var apiPassword))
        {
            return Unauthorized("Missing apiKey or apiPassword headers.");
        }

        var result = await _service.GetByCredentialsAsync(apiKey, apiPassword);
        return result is null ? Unauthorized("Invalid credentials.") : Ok(result);
    }

    /// <summary>
    /// Update application information.
    /// You can update all fields including apiKey and apiPassword.
    /// Requires valid apiKey and apiPassword in the headers.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateApplicationDto dto)
    {
        if (!TryGetCredentials(out var apiKey, out var apiPassword))
        {
            return Unauthorized("Missing apiKey or apiPassword headers.");
        }

        var success = await _service.UpdateAsync(apiKey, apiPassword, dto);
        return success ? Ok() : Unauthorized("Invalid credentials.");
    }

    /// <summary>
    /// Delete an application.
    /// Be careful: deleting an application deletes all related resources.
    /// Requires valid apiKey and apiPassword in the headers.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        if (!TryGetCredentials(out var apiKey, out var apiPassword))
        {
            return Unauthorized("Missing apiKey or apiPassword headers.");
        }

        var success = await _service.DeleteAsync(apiKey, apiPassword);
        return success ? Ok() : Unauthorized("Invalid credentials.");
    }

    // ------------------------------------------------------------
    // Helper: Extract apiKey + apiPassword from HTTP headers
    // ------------------------------------------------------------
    private bool TryGetCredentials(out string apiKey, out string apiPassword)
    {
        apiKey = Request.Headers["apiKey"].FirstOrDefault() ?? string.Empty;
        apiPassword = Request.Headers["apiPassword"].FirstOrDefault() ?? string.Empty;

        return !string.IsNullOrWhiteSpace(apiKey) &&
               !string.IsNullOrWhiteSpace(apiPassword);
    }
}
