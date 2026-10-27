using Gamification.Application.DTOs.Application;
using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.API.Controllers;

/// <summary>
/// Manages the application resource.
/// All operations related to the application resource are in this section.
/// The application is the entry point of the API;
/// all other resources are linked to an application.
/// Each application has a unique apiKey and apiPassword that must be sent
/// in the HTTP headers for authentication.
/// Invalid or missing credentials result in HTTP 401 Unauthorized.
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
    /// Registers a new application. Provide a name, a description, a unique apiKey
    /// and a secure apiPassword. This is the first step before using the API.
    /// Returns the created application with its identifier.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateApplicationDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Created($"/application/{result.Id}", result);
    }

    /// <summary>
    /// Gets application information. You must provide apiKey and apiPassword
    /// in the HTTP headers. Returns the name, description and apiKey of the application.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var result = await _service.GetByCredentialsAsync(apiKey, apiPassword);
        return Ok(result);
    }

    /// <summary>
    /// Updates the information of the application. You can update all information
    /// including apiKey and apiPassword. Requires valid apiKey and apiPassword
    /// in the headers.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateApplicationDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        await _service.UpdateAsync(apiKey, apiPassword, dto);
        return Ok();
    }

    /// <summary>
    /// Deletes an application. Be careful: deleting an application deletes
    /// all related resources in cascade. Requires valid apiKey and apiPassword
    /// in the headers.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        await _service.DeleteAsync(apiKey, apiPassword);
        return Ok();
    }
}
