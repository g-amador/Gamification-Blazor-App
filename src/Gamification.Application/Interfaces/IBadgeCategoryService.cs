using Gamification.Application.DTOs.BadgeCategory;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing badge categories.
/// All operations related to the badge category resource are in this section.
/// A category groups badges together.
/// </summary>
public interface IBadgeCategoryService
{
    /// <summary>
    /// Retrieves the badge categories of the application.
    /// </summary>
    Task<List<BadgeCategoryListItemDto>> GetAllAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Retrieves a badge category of the application by its id.
    /// </summary>
    Task<BadgeCategoryDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int categoryId);

    /// <summary>
    /// Adds a new badge category for the application.
    /// </summary>
    Task<BadgeCategoryCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateBadgeCategoryDto dto);

    /// <summary>
    /// Updates a badge category.
    /// </summary>
    Task<bool> UpdateAsync(string apiKey, string apiPassword, int categoryId, UpdateBadgeCategoryDto dto);
}
