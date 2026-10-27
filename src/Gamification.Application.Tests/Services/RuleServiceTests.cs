using Gamification.Application.DTOs.Rule;
using Gamification.Application.Exceptions;
using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Domain.Entities;
using Moq;

namespace Gamification.Application.Tests.Services;

public class RuleServiceTests
{
    private readonly Mock<IApplicationRepository> _appRepoMock;
    private readonly Mock<IRuleRepository> _ruleRepoMock;
    private readonly Mock<IEventTypeRepository> _eventTypeRepoMock;
    private readonly RuleService _service;

    public RuleServiceTests()
    {
        _appRepoMock = new Mock<IApplicationRepository>();
        _ruleRepoMock = new Mock<IRuleRepository>();
        _eventTypeRepoMock = new Mock<IEventTypeRepository>();
        _service = new RuleService(_appRepoMock.Object, _ruleRepoMock.Object, _eventTypeRepoMock.Object);
    }

    // -------------------------------------------------------------
    // GET ALL
    // -------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_ShouldReturnRules_WhenCredentialsAreValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var rules = new List<RuleEntity>
        {
            new() { Id = 1, BadgeId = 10, NumberOfPoints = 50, EventTypeId = 1, EventType = new EventTypeEntity { Id = 1, Name = "LOGIN" } },
            new() { Id = 2, BadgeId = null, NumberOfPoints = 20, EventTypeId = 2, EventType = new EventTypeEntity { Id = 2, Name = "PURCHASE" } }
        };

        _ruleRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(rules);

        // Act
        var result = await _service.GetAllAsync("key", "pass");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].EventTypeId);
        Assert.Equal("LOGIN", result[0].EventTypeName);
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowUnauthorizedException_WhenCredentialsInvalid()
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
    public async Task GetByIdAsync_ShouldReturnRule_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var rule = new RuleEntity
        {
            Id = 5,
            BadgeId = 10,
            NumberOfPoints = 100,
            EventTypeId = 3,
            EventType = new EventTypeEntity { Id = 3, Name = "LEVEL_UP" }
        };

        _ruleRepoMock.Setup(r => r.GetByIdAsync(1, 5)).ReturnsAsync(rule);

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result!.EventTypeId);
        Assert.Equal("LEVEL_UP", result.EventTypeName);
        Assert.Equal(100, result.NumberOfPoints);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenRuleNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _ruleRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                     .ReturnsAsync((RuleEntity?)null);

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowUnauthorizedException_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _service.GetByIdAsync("bad", "creds", 1));
    }

    // -------------------------------------------------------------
    // CREATE
    // -------------------------------------------------------------

    [Fact]
    public async Task CreateAsync_ShouldCreateRule_WhenCredentialsValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _eventTypeRepoMock.Setup(r => r.GetByIdAsync(1, 1))
                          .ReturnsAsync(new EventTypeEntity { Id = 1, Name = "LOGIN" });

        var dto = new CreateRuleDto
        {
            BadgeId = 10,
            NumberOfPoints = 50,
            EventTypeId = 1
        };

        // Act
        var result = await _service.CreateAsync("key", "pass", dto);

        // Assert
        Assert.Equal("created", result.Status);
        _ruleRepoMock.Verify(r => r.AddAsync(It.IsAny<RuleEntity>()), Times.Once);
        _ruleRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenEventTypeNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _eventTypeRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                          .ReturnsAsync((EventTypeEntity?)null);

        var dto = new CreateRuleDto
        {
            NumberOfPoints = 50,
            EventTypeId = 99
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync("key", "pass", dto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowUnauthorizedException_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        var dto = new CreateRuleDto();

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _service.CreateAsync("bad", "creds", dto));
    }

    // -------------------------------------------------------------
    // UPDATE
    // -------------------------------------------------------------

    [Fact]
    public async Task UpdateAsync_ShouldUpdateRule_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var rule = new RuleEntity { Id = 10, NumberOfPoints = 10 };
        _ruleRepoMock.Setup(r => r.GetByIdAsync(1, 10)).ReturnsAsync(rule);

        _eventTypeRepoMock.Setup(r => r.GetByIdAsync(1, 2))
                          .ReturnsAsync(new EventTypeEntity { Id = 2, Name = "PURCHASE" });

        var dto = new UpdateRuleDto
        {
            BadgeId = 20,
            NumberOfPoints = 200,
            EventTypeId = 2
        };

        // Act
        var result = await _service.UpdateAsync("key", "pass", 10, dto);

        // Assert
        Assert.True(result);
        Assert.Equal(2, rule.EventTypeId);
        _ruleRepoMock.Verify(r => r.UpdateAsync(rule), Times.Once);
        _ruleRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenRuleNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _ruleRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                     .ReturnsAsync((RuleEntity?)null);

        var dto = new UpdateRuleDto();

        // Act
        var result = await _service.UpdateAsync("key", "pass", 99, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowUnauthorizedException_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        var dto = new UpdateRuleDto();

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _service.UpdateAsync("bad", "creds", 1, dto));
    }

    // -------------------------------------------------------------
    // DELETE
    // -------------------------------------------------------------

    [Fact]
    public async Task DeleteAsync_ShouldDeleteRule_WhenValid()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        var rule = new RuleEntity { Id = 10 };
        _ruleRepoMock.Setup(r => r.GetByIdAsync(1, 10)).ReturnsAsync(rule);

        // Act
        var result = await _service.DeleteAsync("key", "pass", 10);

        // Assert
        Assert.True(result);
        _ruleRepoMock.Verify(r => r.DeleteAsync(rule), Times.Once);
        _ruleRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenRuleNotFound()
    {
        // Arrange
        var app = new ApplicationEntity { Id = 1 };
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("key", "pass"))
                    .ReturnsAsync(app);

        _ruleRepoMock.Setup(r => r.GetByIdAsync(1, 99))
                     .ReturnsAsync((RuleEntity?)null);

        // Act
        var result = await _service.DeleteAsync("key", "pass", 99);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowUnauthorizedException_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _service.DeleteAsync("bad", "creds", 1));
    }
}
