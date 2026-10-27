using Gamification.Application.DTOs.Application;
using Gamification.Application.Exceptions;
using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;

namespace Gamification.Application.Services;

/// <inheritdoc />
/// <summary>
/// Implements operations for managing applications.
/// The application is the entry point of the API.
/// All other resources are linked to an application.
/// Each application has a unique apiKey and apiPassword
/// that must be provided in the HTTP headers for authentication.
/// </summary>
public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _repo;

    public ApplicationService(IApplicationRepository repo)
    {
        _repo = repo;
    }

    /// <inheritdoc />
    public async Task<List<ApplicationListItemDto>> GetAllAsync()
    {
        // Load all applications registered on the server
        var entities = await _repo.GetAllAsync();

        // Map to summary DTOs (name, description and key are never exposed in lists)
        return entities.Select(a => new ApplicationListItemDto
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<ApplicationCreatedResponseDto> CreateAsync(CreateApplicationDto dto)
    {
        // Ensure apiKey is unique
        if (await _repo.ApiKeyExistsAsync(dto.ApiKey))
        {
            throw new InvalidOperationException("apiKey must be unique.");
        }

        // Ensure apiPassword is unique
        if (await _repo.ApiPasswordExistsAsync(dto.ApiPassword))
        {
            throw new InvalidOperationException("apiPassword must be unique.");
        }

        // Create entity
        var entity = new ApplicationEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            ApiKey = dto.ApiKey,
            ApiPassword = dto.ApiPassword
        };

        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();

        return new ApplicationCreatedResponseDto
        {
            Status = "created",
            Url = $"/application/{entity.Id}",
            Id = entity.Id
        };
    }

    /// <inheritdoc />
    public async Task<ApplicationDto?> GetByCredentialsAsync(string apiKey, string apiPassword)
    {
        // Retrieve entity by credentials
        var entity = await _repo.GetByCredentialsAsync(apiKey, apiPassword);

        if (entity is null)
        {
            throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");
        }

        // Map entity to DTO
        return new ApplicationDto
        {
            Name = entity.Name,
            Description = entity.Description,
            ApiKey = entity.ApiKey
        };
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(string apiKey, string apiPassword, UpdateApplicationDto dto)
    {
        // Find the application being updated
        var entity = await _repo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Check apiKey uniqueness (excluding current app)
        if (entity.ApiKey != dto.ApiKey &&
            await _repo.ApiKeyExistsAsync(dto.ApiKey))
        {
            throw new InvalidOperationException("apiKey must be unique.");
        }

        // Check apiPassword uniqueness (excluding current app)
        if (entity.ApiPassword != dto.ApiPassword &&
            await _repo.ApiPasswordExistsAsync(dto.ApiPassword))
        {
            throw new InvalidOperationException("apiPassword must be unique.");
        }

        // Apply updates
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.ApiKey = dto.ApiKey;
        entity.ApiPassword = dto.ApiPassword;

        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string apiKey, string apiPassword)
    {
        // Retrieve entity by credentials
        var entity = await _repo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Delete entity
        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();

        return true;
    }
}
