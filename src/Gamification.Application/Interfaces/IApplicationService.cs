using Gamification.Application.DTOs;

namespace Gamification.Application.Interfaces;

/// <summary>
/// Defines operations for managing applications.
/// </summary>
public interface IApplicationService
{
    /// <summary>
    /// Retrieves an application by its identifier.
    /// </summary>
    Task<ApplicationDto?> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new application.
    /// </summary>
    Task<ApplicationCreatedResponseDto> CreateAsync(CreateApplicationDto dto);

    /// <summary>
    /// Updates an existing application.
    /// </summary>
    Task<bool> UpdateAsync(int id, UpdateApplicationDto dto);

    /// <summary>
    /// Deletes an application by its identifier.
    /// </summary>
    Task<bool> DeleteAsync(int id);
}
