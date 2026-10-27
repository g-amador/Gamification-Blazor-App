using Gamification.Application.DTOs.EventType;
using Gamification.Application.Exceptions;
using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Domain.Entities;
using Moq;

namespace Gamification.Application.Tests.Services;

public class EventTypeServiceTests
{
    private readonly Mock<IApplicationRepository> _appRepoMock;
    private readonly Mock<IEventTypeRepository> _eventTypeRepoMock;
    private readonly EventTypeService _service;

    public EventTypeServiceTests()
    {
        _appRepoMock = new Mock<IApplicationRepository>();
        _eventTypeRepoMock = new Mock<IEventTypeRepository>();

        _service = new EventTypeService(_appRepoMock.Object, _eventTypeRepoMock.Object);
    }

    // -------------------------------------------------------------
    // GET ALL
    // -------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_ShouldReturnEventTypes_WhenCredentialsAreValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var eventTypes = new List<EventTypeEntity>
        {
            new() { Id = 1, Name = "LOGIN", ApplicationId = 1 },
            new() { Id = 2, Name = "PURCHASE", ApplicationId = 1 }
        };

        _eventTypeRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(eventTypes);

        // Act
        var result = await _service.GetAllAsync("key", "pass");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("LOGIN", result[0].Name);
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
    public async Task GetByIdAsync_ShouldReturnEventType_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _eventTypeRepoMock.Setup(r => r.GetByIdAsync(1, 2))
                          .ReturnsAsync(new EventTypeEntity { Id = 2, Name = "PURCHASE", ApplicationId = 1 });

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("PURCHASE", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEventTypeNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _eventTypeRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                          .ReturnsAsync((EventTypeEntity?)null);

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 99);

        // Assert
        Assert.Null(result);
    }

    // -------------------------------------------------------------
    // CREATE
    // -------------------------------------------------------------

    [Fact]
    public async Task CreateAsync_ShouldCreateEventType_WhenNameIsUnique()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);
        _eventTypeRepoMock.Setup(r => r.NameExistsAsync(1, "LOGIN")).ReturnsAsync(false);

        var dto = new CreateEventTypeDto { Name = "LOGIN" };

        // Act
        var result = await _service.CreateAsync("key", "pass", dto);

        // Assert
        Assert.Equal("created", result.Status);
        Assert.Equal("/event-types/0", result.Url);

        _eventTypeRepoMock.Verify(r => r.AddAsync(It.IsAny<EventTypeEntity>()), Times.Once);
        _eventTypeRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenNameAlreadyExists()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);
        _eventTypeRepoMock.Setup(r => r.NameExistsAsync(1, "LOGIN")).ReturnsAsync(true);

        var dto = new CreateEventTypeDto { Name = "LOGIN" };

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

        var dto = new CreateEventTypeDto { Name = "LOGIN" };

        // Act + Assert
        await Assert.ThrowsAsync<UnauthorizedException>(
            () => _service.CreateAsync("bad", "creds", dto));
    }

    // -------------------------------------------------------------
    // UPDATE
    // -------------------------------------------------------------

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEventType_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var entity = new EventTypeEntity { Id = 2, Name = "LOGIN", ApplicationId = 1 };
        _eventTypeRepoMock.Setup(r => r.GetByIdAsync(1, 2)).ReturnsAsync(entity);
        _eventTypeRepoMock.Setup(r => r.NameExistsAsync(1, "LOGIN_EVENT")).ReturnsAsync(false);

        var dto = new UpdateEventTypeDto { Name = "LOGIN_EVENT" };

        // Act
        var result = await _service.UpdateAsync("key", "pass", 2, dto);

        // Assert
        Assert.True(result);
        Assert.Equal("LOGIN_EVENT", entity.Name);

        _eventTypeRepoMock.Verify(r => r.UpdateAsync(entity), Times.Once);
        _eventTypeRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenEventTypeNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _eventTypeRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                          .ReturnsAsync((EventTypeEntity?)null);

        var dto = new UpdateEventTypeDto { Name = "LOGIN" };

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

        var entity = new EventTypeEntity { Id = 2, Name = "LOGIN", ApplicationId = 1 };
        _eventTypeRepoMock.Setup(r => r.GetByIdAsync(1, 2)).ReturnsAsync(entity);
        _eventTypeRepoMock.Setup(r => r.NameExistsAsync(1, "PURCHASE")).ReturnsAsync(true);

        var dto = new UpdateEventTypeDto { Name = "PURCHASE" };

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync("key", "pass", 2, dto));
    }
}
