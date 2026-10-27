using Gamification.Application.DTOs.BadgeCategory;
using Gamification.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

/// <summary>
/// Manages the badge categories of an application.
/// All operations related to the badge category resource are in this section.
/// A category groups badges together.
/// </summary>
[ApiController]
[Route("badge-categories")]
public class BadgeCategoriesController : ControllerBase
{
    private readonly IBadgeCategoryService _service;

    public BadgeCategoriesController(IBadgeCategoryService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns the badge categories of the application.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var categories = await _service.GetAllAsync(apiKey, apiPassword);
        return Ok(new { items = categories });
    }

    /// <summary>
    /// Adds a new badge category for the application.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBadgeCategoryDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var result = await _service.CreateAsync(apiKey, apiPassword, dto);
        return Created(result.Url, result);
    }

    /// <summary>
    /// Returns a badge category of the application.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var category = await _service.GetByIdAsync(apiKey, apiPassword, id);
        if (category is null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    /// <summary>
    /// Updates a badge category.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBadgeCategoryDto dto)
    {
        var apiKey = Request.Headers["apiKey"].ToString();
        var apiPassword = Request.Headers["apiPassword"].ToString();

        var success = await _service.UpdateAsync(apiKey, apiPassword, id, dto);
        return success ? Ok() : NotFound();
    }
}
