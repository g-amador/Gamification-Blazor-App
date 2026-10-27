using Gamification.Application.DTOs.Badge;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing badges.
/// All operations related to the badge resource are in this section.
/// A badge is a distinction, won by a player when he does something significant (an event).
/// </summary>
public interface IBadgeService
{
    /// <summary>
    /// Retrieves the badges of the application.
    /// </summary>
    Task<List<BadgeListItemDto>> GetAllAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Retrieves information of the given badge, including categoryId when set.
    /// </summary>
    Task<BadgeDto?> GetByIdAsync(string apiKey, string apiPassword, int badgeId);

    /// <summary>
    /// Adds a new badge for the application. categoryId is optional.
    /// </summary>
    Task<BadgeCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateBadgeDto dto);

    /// <summary>
    /// Updates a badge. categoryId is optional.
    /// </summary>
    Task<bool> UpdateAsync(string apiKey, string apiPassword, int badgeId, UpdateBadgeDto dto);

    /// <summary>
    /// Deletes a badge. All links to this badge by rules are set to null.
    /// </summary>
    Task<bool> DeleteAsync(string apiKey, string apiPassword, int badgeId);
}
