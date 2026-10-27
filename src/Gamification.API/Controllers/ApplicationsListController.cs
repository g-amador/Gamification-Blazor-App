using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>
/// Lists the applications registered on the server.
/// The applications are the entry point of the API;
/// all other resources are linked to an application.
/// </summary>
[ApiController]
[Route("applications")]
public class ApplicationsListController : ControllerBase
{
    private readonly IApplicationService _service;

    public ApplicationsListController(IApplicationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns the list of all applications registered on the server.
    /// Use valid apiKey and apiPassword credentials in the HTTP headers to access this resource.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        // Invalid or missing credentials result in 401 Unauthorized (global exception handling)
        await _service.GetByCredentialsAsync(apiKey, apiPassword);

        var applications = await _service.GetAllAsync();
        return Ok(new { items = applications });
    }
}
