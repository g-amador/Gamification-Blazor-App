using Gamification.Application.DTOs.Application;
using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Domain.Entities;
using Moq;

namespace Gamification.Application.Tests.Services;

public class ApplicationServiceTests
{
    private readonly Mock<IApplicationRepository> _repoMock;
    private readonly ApplicationService _service;

    public ApplicationServiceTests()
    {
        _repoMock = new Mock<IApplicationRepository>();
        _service = new ApplicationService(_repoMock.Object);
    }

    // -------------------------------------------------------------
    // CREATE
    // -------------------------------------------------------------

    [Fact]
    public async Task CreateAsync_ShouldCreateApplication_WhenApiKeyAndPasswordAreUnique()
    {
        // Arrange
        var dto = new CreateApplicationDto
        {
            Name = "Test App",
            Description = "Desc",
            ApiKey = "key123",
            ApiPassword = "pass123"
        };

        _repoMock.Setup(r => r.ApiKeyExistsAsync(dto.ApiKey)).ReturnsAsync(false);
        _repoMock.Setup(r => r.ApiPasswordExistsAsync(dto.ApiPassword)).ReturnsAsync(false);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.Equal("created", result.Status);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<ApplicationEntity>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenApiKeyIsNotUnique()
    {
        // Arrange
        var dto = new CreateApplicationDto
        {
            Name = "Test",
            Description = "Desc",
            ApiKey = "duplicate",
            ApiPassword = "pass123"
        };

        _repoMock.Setup(r => r.ApiKeyExistsAsync(dto.ApiKey)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenApiPasswordIsNotUnique()
    {
        // Arrange
        var dto = new CreateApplicationDto
        {
            Name = "Test",
            Description = "Desc",
            ApiKey = "key123",
            ApiPassword = "duplicate"
        };

        _repoMock.Setup(r => r.ApiKeyExistsAsync(dto.ApiKey)).ReturnsAsync(false);
        _repoMock.Setup(r => r.ApiPasswordExistsAsync(dto.ApiPassword)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }

    // -------------------------------------------------------------
    // GET
    // -------------------------------------------------------------

    [Fact]
    public async Task GetByCredentialsAsync_ShouldReturnDto_WhenCredentialsAreValid()
    {
        // Arrange
        var entity = new ApplicationEntity
        {
            Id = 1,
            Name = "App",
            Description = "Desc",
            ApiKey = "key",
            ApiPassword = "pass"
        };

        _repoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                 .ReturnsAsync(entity);

        // Act
        var result = await _service.GetByCredentialsAsync("key", "pass");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("App", result!.Name);
        Assert.Equal("key", result.ApiKey);
    }

    [Fact]
    public async Task GetByCredentialsAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                 .ReturnsAsync((ApplicationEntity?)null);

        // Act
        var result = await _service.GetByCredentialsAsync("bad", "creds");

        // Assert
        Assert.Null(result);
    }

    // -------------------------------------------------------------
    // UPDATE
    // -------------------------------------------------------------

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenCredentialsAreValidAndUnique()
    {
        // Arrange
        var existing = new ApplicationEntity
        {
            Id = 1,
            Name = "Old",
            Description = "Old",
            ApiKey = "oldKey",
            ApiPassword = "oldPass"
        };

        var dto = new UpdateApplicationDto
        {
            Name = "New",
            Description = "New",
            ApiKey = "newKey",
            ApiPassword = "newPass"
        };

        _repoMock.Setup(r => r.GetByCredentialsAsync("oldKey", "oldPass"))
                 .ReturnsAsync(existing);

        _repoMock.Setup(r => r.ApiKeyExistsAsync("newKey")).ReturnsAsync(false);
        _repoMock.Setup(r => r.ApiPasswordExistsAsync("newPass")).ReturnsAsync(false);

        // Act
        var result = await _service.UpdateAsync("oldKey", "oldPass", dto);

        // Assert
        Assert.True(result);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenCredentialsAreInvalid()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                 .ReturnsAsync((ApplicationEntity?)null);

        var dto = new UpdateApplicationDto();

        // Act
        var result = await _service.UpdateAsync("bad", "creds", dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenNewApiKeyIsDuplicate()
    {
        // Arrange
        var existing = new ApplicationEntity
        {
            Id = 1,
            ApiKey = "oldKey",
            ApiPassword = "oldPass"
        };

        var dto = new UpdateApplicationDto
        {
            ApiKey = "duplicate",
            ApiPassword = "newPass"
        };

        _repoMock.Setup(r => r.GetByCredentialsAsync("oldKey", "oldPass"))
                 .ReturnsAsync(existing);

        _repoMock.Setup(r => r.ApiKeyExistsAsync("duplicate")).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync("oldKey", "oldPass", dto));
    }

    // -------------------------------------------------------------
    // DELETE
    // -------------------------------------------------------------

    [Fact]
    public async Task DeleteAsync_ShouldDelete_WhenCredentialsAreValid()
    {
        // Arrange
        var entity = new ApplicationEntity { Id = 1 };

        _repoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                 .ReturnsAsync(entity);

        // Act
        var result = await _service.DeleteAsync("key", "pass");

        // Assert
        Assert.True(result);
        _repoMock.Verify(r => r.DeleteAsync(entity), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenCredentialsAreInvalid()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                 .ReturnsAsync((ApplicationEntity?)null);

        // Act
        var result = await _service.DeleteAsync("bad", "creds");

        // Assert
        Assert.False(result);
    }
}
