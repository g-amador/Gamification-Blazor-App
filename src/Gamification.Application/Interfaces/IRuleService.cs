using Gamification.Application.DTOs.Rule;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing rules.
/// </summary>
public interface IRuleService
{
    /// <summary>
    /// Retrieves all rules of an application.
    /// </summary>
    Task<List<RuleListItemDto>> GetAllAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Retrieves detailed information about a rule.
    /// </summary>
    Task<RuleDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int ruleId);

    /// <summary>
    /// Creates a new rule.
    /// </summary>
    Task<RuleCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateRuleDto dto);

    /// <summary>
    /// Updates a rule.
    /// </summary>
    Task<bool> UpdateAsync(string apiKey, string apiPassword, int ruleId, UpdateRuleDto dto);

    /// <summary>
    /// Deletes a rule.
    /// </summary>
    Task<bool> DeleteAsync(string apiKey, string apiPassword, int ruleId);
}
