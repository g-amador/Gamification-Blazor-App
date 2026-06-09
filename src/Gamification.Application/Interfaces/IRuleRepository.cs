using Gamification.Domain.Entities;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Provides persistence operations for rules.
/// </summary>
public interface IRuleRepository
{
    /// <summary>
    /// Retrieves all rules of an application.
    /// </summary>
    Task<List<RuleEntity>> GetAllAsync(int applicationId);

    /// <summary>
    /// Retrieves a rule by id within an application.
    /// </summary>
    Task<RuleEntity?> GetByIdAsync(int applicationId, int ruleId);

    /// <summary>
    /// Adds a new rule.
    /// </summary>
    Task AddAsync(RuleEntity entity);

    /// <summary>
    /// Updates an existing rule.
    /// </summary>
    Task UpdateAsync(RuleEntity entity);

    /// <summary>
    /// Deletes a rule.
    /// </summary>
    Task DeleteAsync(RuleEntity entity);

    /// <summary>
    /// Saves changes to the data store.
    /// </summary>
    Task SaveChangesAsync();
}
