using Gamification.Application.DTOs.Event;
using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>
/// Manages the events of an application.
/// All operations related to the event resource are in this section.
/// Everything that has to be monitored by the gamification platform is recorded
/// as an event and has to be posted to the gamification API.
/// When something happens to a player or when a player does something, an event
/// should be created. An event is linked to a player and is of a type.
/// Multiple events of the same type and player can be created.
/// An event cannot be updated or deleted.
/// </summary>
[ApiController]
[Route("events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;

    public EventsController(IEventService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns the list of all events registered on the gamification server for the application.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var events = await _service.GetAllAsync(apiKey, apiPassword);
        return Ok(new { items = events });
    }

    /// <summary>
    /// Registers a new event on the gamification platform for the application.
    /// When an event is created, the rules are browsed and the corresponding
    /// points and badges are given to the player.
    /// If a badge is already won by the player, nothing happens. Returns the event's id.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var result = await _service.CreateAsync(apiKey, apiPassword, dto);
        return Created(result.Url, result);
    }

    /// <summary>
    /// Returns detailed information about a specific event identified by its id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var ev = await _service.GetByIdAsync(apiKey, apiPassword, id);
        if (ev is null)
        {
            return NotFound();
        }

        return Ok(ev);
    }
}
