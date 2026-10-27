using Gamification.Application.DTOs.BadgeCategory;
using Gamification.Application.Exceptions;
using Gamification.Application.Interfaces;
using Gamification.Domain.Entities;

namespace Gamification.Application.Services;

/// <summary>
/// Implements operations for managing badge categories.
/// A category groups badges together.
/// </summary>
public class BadgeCategoryService : IBadgeCategoryService
{
    private readonly IApplicationRepository _appRepo;
    private readonly IBadgeCategoryRepository _categoryRepo;

    public BadgeCategoryService(IApplicationRepository appRepo, IBadgeCategoryRepository categoryRepo)
    {
        _appRepo = appRepo;
        _categoryRepo = categoryRepo;
    }

    /// <inheritdoc />
    public async Task<List<BadgeCategoryListItemDto>> GetAllAsync(string apiKey, string apiPassword)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load badge categories
        var categories = await _categoryRepo.GetAllAsync(app.Id);

        // Map to DTOs
        return categories.Select(c => new BadgeCategoryListItemDto
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<BadgeCategoryDetailsDto?> GetByIdAsync(string apiKey, string apiPassword, int categoryId)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load badge category
        var category = await _categoryRepo.GetByIdAsync(app.Id, categoryId);
        if (category is null)
        {
            return null;
        }

        // Map to DTO
        return new BadgeCategoryDetailsDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    /// <inheritdoc />
    public async Task<BadgeCategoryCreatedResponseDto> CreateAsync(string apiKey, string apiPassword, CreateBadgeCategoryDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Ensure the badge category name is unique within the application
        if (await _categoryRepo.NameExistsAsync(app.Id, dto.Name))
        {
            throw new InvalidOperationException("A badge category with this name already exists for the application.");
        }

        // Create entity
        var entity = new BadgeCategoryEntity
        {
            Name = dto.Name,
            ApplicationId = app.Id
        };

        // Save
        await _categoryRepo.AddAsync(entity);
        await _categoryRepo.SaveChangesAsync();

        return new BadgeCategoryCreatedResponseDto
        {
            Status = "created",
            Url = $"/badge-categories/{entity.Id}",
            Id = entity.Id
        };
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(string apiKey, string apiPassword, int categoryId, UpdateBadgeCategoryDto dto)
    {
        // Validate application credentials
        var app = await _appRepo.GetByCredentialsAsync(apiKey, apiPassword)
            ?? throw new UnauthorizedException("Invalid or missing apiKey/apiPassword credentials.");

        // Load badge category
        var category = await _categoryRepo.GetByIdAsync(app.Id, categoryId);
        if (category is null)
        {
            return false;
        }

        // Ensure the badge category name is unique within the application (excluding itself)
        if (category.Name != dto.Name &&
            await _categoryRepo.NameExistsAsync(app.Id, dto.Name))
        {
            throw new InvalidOperationException("A badge category with this name already exists for the application.");
        }

        // Update and save
        category.Name = dto.Name;
        await _categoryRepo.UpdateAsync(category);
        await _categoryRepo.SaveChangesAsync();

        return true;
    }
}
