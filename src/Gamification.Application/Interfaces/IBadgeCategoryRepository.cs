using Gamification.Domain.Entities;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Provides persistence operations for badge categories.
/// All operations related to the badge category resource are in this section.
/// A category groups badges together.
/// </summary>
public interface IBadgeCategoryRepository
{
    /// <summary>
    /// Retrieves all badge categories belonging to an application.
    /// </summary>
    Task<List<BadgeCategoryEntity>> GetAllAsync(int applicationId);

    /// <summary>
    /// Retrieves a badge category by id within an application.
    /// </summary>
    Task<BadgeCategoryEntity?> GetByIdAsync(int applicationId, int categoryId);

    /// <summary>
    /// Adds a new badge category.
    /// </summary>
    Task AddAsync(BadgeCategoryEntity entity);

    /// <summary>
    /// Updates an existing badge category.
    /// </summary>
    Task UpdateAsync(BadgeCategoryEntity entity);

    /// <summary>
    /// Saves changes to the data store.
    /// </summary>
    Task SaveChangesAsync();

    /// <summary>
    /// Checks whether a badge category with the given name already exists in the application.
    /// </summary>
    Task<bool> NameExistsAsync(int applicationId, string name);
}
