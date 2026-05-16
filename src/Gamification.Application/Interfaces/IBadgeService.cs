using Gamification.Application.DTOs.Badge;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing badges.
/// </summary>
public interface IBadgeService
{
    /// <summary>
    /// Retrieves all badges of an application.
    /// </summary>
    Task<List<BadgeListItemDto>> GetAllAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Retrieves detailed information about a badge.
    /// </summary>
    Task<BadgeDto?> GetByIdAsync(string apiKey, string apiPassword, int badgeId);

    /// <summary>
    /// Creates a new badge.
    /// </summary>
    Task<BadgeCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateBadgeDto dto);

    /// <summary>
    /// Updates a badge.
    /// </summary>
    Task<bool> UpdateAsync(string apiKey, string apiPassword, int badgeId, UpdateBadgeDto dto);

    /// <summary>
    /// Deletes a badge.
    /// </summary>
    Task<bool> DeleteAsync(string apiKey, string apiPassword, int badgeId);
}
