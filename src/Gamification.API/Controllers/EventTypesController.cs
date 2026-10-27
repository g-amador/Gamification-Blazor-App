using Gamification.Application.DTOs.EventType;
using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>
/// Manages the event types of an application.
/// All operations related to the event type resource are in this section.
/// All events that are recorded by the gamification API are associated with a type.
/// The types can be defined by the application using the API with the related API calls.
/// Then event types can be used in rules to define the conditions needed to obtain a badge.
/// </summary>
[ApiController]
[Route("event-types")]
public class EventTypesController : ControllerBase
{
    private readonly IEventTypeService _service;

    public EventTypesController(IEventTypeService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns the list of all event types defined for the application.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var eventTypes = await _service.GetAllAsync(apiKey, apiPassword);
        return Ok(new { items = eventTypes });
    }

    /// <summary>
    /// Adds a new event type in the application.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventTypeDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var result = await _service.CreateAsync(apiKey, apiPassword, dto);
        return Created(result.Url, result);
    }

    /// <summary>
    /// Returns information about a specific event type id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var eventType = await _service.GetByIdAsync(apiKey, apiPassword, id);
        if (eventType is null)
        {
            return NotFound();
        }

        return Ok(eventType);
    }

    /// <summary>
    /// Updates the information of the given event type.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEventTypeDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var success = await _service.UpdateAsync(apiKey, apiPassword, id, dto);
        return success ? Ok() : NotFound();
    }
}
