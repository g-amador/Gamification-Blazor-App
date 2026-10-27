using Gamification.Application.DTOs.BadgeCategory;
using Gamification.Application.Exceptions;
using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Domain.Entities;
using Moq;

namespace Gamification.Application.Tests.Services;

public class BadgeCategoryServiceTests
{
    private readonly Mock<IApplicationRepository> _appRepoMock;
    private readonly Mock<IBadgeCategoryRepository> _categoryRepoMock;
    private readonly BadgeCategoryService _service;

    public BadgeCategoryServiceTests()
    {
        _appRepoMock = new Mock<IApplicationRepository>();
        _categoryRepoMock = new Mock<IBadgeCategoryRepository>();

        _service = new BadgeCategoryService(_appRepoMock.Object, _categoryRepoMock.Object);
    }

    // -------------------------------------------------------------
    // GET ALL
    // -------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_ShouldReturnCategories_WhenCredentialsAreValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var categories = new List<BadgeCategoryEntity>
        {
            new() { Id = 1, Name = "Competence", ApplicationId = 1 },
            new() { Id = 2, Name = "Teamwork", ApplicationId = 1 }
        };

        _categoryRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(categories);

        // Act
        var result = await _service.GetAllAsync("key", "pass");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Competence", result[0].Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowUnauthorizedException_WhenCredentialsAreInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _service.GetAllAsync("bad", "creds"));
    }

    // -------------------------------------------------------------
    // GET BY ID
    // -------------------------------------------------------------

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _categoryRepoMock.Setup(r => r.GetByIdAsync(1, 2))
                         .ReturnsAsync(new BadgeCategoryEntity { Id = 2, Name = "Teamwork", ApplicationId = 1 });

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Teamwork", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCategoryNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _categoryRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                         .ReturnsAsync((BadgeCategoryEntity?)null);

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 99);

        // Assert
        Assert.Null(result);
    }

    // -------------------------------------------------------------
    // CREATE
    // -------------------------------------------------------------

    [Fact]
    public async Task CreateAsync_ShouldCreateCategory_WhenNameIsUnique()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);
        _categoryRepoMock.Setup(r => r.NameExistsAsync(1, "Competence")).ReturnsAsync(false);

        var dto = new CreateBadgeCategoryDto { Name = "Competence" };

        // Act
        var result = await _service.CreateAsync("key", "pass", dto);

        // Assert
        Assert.Equal("created", result.Status);
        Assert.Equal("/badge-categories/0", result.Url);

        _categoryRepoMock.Verify(r => r.AddAsync(It.IsAny<BadgeCategoryEntity>()), Times.Once);
        _categoryRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenNameAlreadyExists()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);
        _categoryRepoMock.Setup(r => r.NameExistsAsync(1, "Competence")).ReturnsAsync(true);

        var dto = new CreateBadgeCategoryDto { Name = "Competence" };

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync("key", "pass", dto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowUnauthorizedException_WhenCredentialsAreInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        var dto = new CreateBadgeCategoryDto { Name = "Competence" };

        // Act + Assert
        await Assert.ThrowsAsync<UnauthorizedException>(
            () => _service.CreateAsync("bad", "creds", dto));
    }

    // -------------------------------------------------------------
    // UPDATE
    // -------------------------------------------------------------

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategory_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var entity = new BadgeCategoryEntity { Id = 2, Name = "Competence", ApplicationId = 1 };
        _categoryRepoMock.Setup(r => r.GetByIdAsync(1, 2)).ReturnsAsync(entity);
        _categoryRepoMock.Setup(r => r.NameExistsAsync(1, "Know-how")).ReturnsAsync(false);

        var dto = new UpdateBadgeCategoryDto { Name = "Know-how" };

        // Act
        var result = await _service.UpdateAsync("key", "pass", 2, dto);

        // Assert
        Assert.True(result);
        Assert.Equal("Know-how", entity.Name);

        _categoryRepoMock.Verify(r => r.UpdateAsync(entity), Times.Once);
        _categoryRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenCategoryNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _categoryRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                         .ReturnsAsync((BadgeCategoryEntity?)null);

        var dto = new UpdateBadgeCategoryDto { Name = "Know-how" };

        // Act
        var result = await _service.UpdateAsync("key", "pass", 99, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenNewNameAlreadyExists()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var entity = new BadgeCategoryEntity { Id = 2, Name = "Competence", ApplicationId = 1 };
        _categoryRepoMock.Setup(r => r.GetByIdAsync(1, 2)).ReturnsAsync(entity);
        _categoryRepoMock.Setup(r => r.NameExistsAsync(1, "Teamwork")).ReturnsAsync(true);

        var dto = new UpdateBadgeCategoryDto { Name = "Teamwork" };

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync("key", "pass", 2, dto));
    }
}
