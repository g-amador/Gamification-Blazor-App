using Gamification.Application.DTOs.Application;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing applications.
/// The application is the entry point of the API.
/// All other resources are linked to an application.
/// Each application has a unique apiKey and apiPassword
/// that must be provided in the HTTP headers for authentication.
/// </summary>
public interface IApplicationService
{
    /// <summary>
    /// Creates a new application.
    /// This is the first operation required before using the API.
    /// </summary>
    Task<ApplicationCreatedResponseDto> CreateAsync(CreateApplicationDto dto);

    /// <summary>
    /// Retrieves an application using its apiKey and apiPassword.
    /// Returns the name, description and key of the application.
    /// Returns null if the credentials are invalid.
    /// </summary>
    Task<ApplicationDto?> GetByCredentialsAsync(string apiKey, string apiPassword);

    /// <summary>
    /// Updates an application using its apiKey and apiPassword.
    /// All fields can be updated, including apiKey and apiPassword.
    /// Returns false if the credentials are invalid.
    /// </summary>
    Task<bool> UpdateAsync(string apiKey, string apiPassword, UpdateApplicationDto dto);

    /// <summary>
    /// Deletes an application using its apiKey and apiPassword.
    /// Deleting an application removes all related resources in cascade.
    /// Returns false if the credentials are invalid.
    /// </summary>
    Task<bool> DeleteAsync(string apiKey, string apiPassword);
}
