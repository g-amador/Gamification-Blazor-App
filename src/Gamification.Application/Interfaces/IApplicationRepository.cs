using Gamification.Domain.Entities;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Provides persistence operations for applications.
/// </summary>
public interface IApplicationRepository
{
    /// <summary>
    /// Retrieves an application using its apiKey and apiPassword.
    /// </summary>
    Task<ApplicationEntity?> GetByCredentialsAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Retrieves an application by its identifier.
    /// </summary>
    Task<ApplicationEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new application to the data store.
    /// </summary>
    Task AddAsync(ApplicationEntity entity);

    /// <summary>
    /// Updates an existing application.
    /// </summary>
    Task UpdateAsync(ApplicationEntity entity);

    /// <summary>
    /// Deletes an application.
    /// </summary>
    Task DeleteAsync(ApplicationEntity entity);

    /// <summary>
    /// Persists changes to the data store.
    /// </summary>
    Task SaveChangesAsync();

    /// <summary>
    /// Checks whether an apiKey already exists.
    /// </summary>
    Task<bool> ApiKeyExistsAsync(string apiKey);

    /// <summary>
    /// Checks whether an apiPassword already exists.
    /// </summary>
    Task<bool> ApiPasswordExistsAsync(string apiPassword);
}
