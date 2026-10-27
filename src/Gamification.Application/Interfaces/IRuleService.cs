using Gamification.Application.DTOs.Rule;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing rules.
/// All operations related to the rule resource are in this section.
/// A rule defines when badges or points should be granted to a player.
/// Many rules can be created for the same event type.
/// </summary>
public interface IRuleService
{
    /// <summary>
    /// Retrieves the rules defined for the application.
    /// </summary>
    Task<List<RuleListItemDto>> GetAllAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Retrieves information about a specific rule.
    /// </summary>
    Task<RuleDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int ruleId);

    /// <summary>
    /// Adds a new rule for the application. The badgeId is optional,
    /// the number of points is required but can be set to 0.
    /// </summary>
    Task<RuleCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateRuleDto dto);

    /// <summary>
    /// Updates a rule. This doesn't change badges and points given to players on past events.
    /// </summary>
    Task<bool> UpdateAsync(string apiKey, string apiPassword, int ruleId, UpdateRuleDto dto);

    /// <summary>
    /// Deletes a rule. This doesn't remove badges and points given to players on past events.
    /// </summary>
    Task<bool> DeleteAsync(string apiKey, string apiPassword, int ruleId);
}
