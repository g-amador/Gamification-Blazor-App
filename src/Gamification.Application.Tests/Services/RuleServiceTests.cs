using Gamification.Application.DTOs.Rule;
using Gamification.Application.Interfaces;
using Gamification.Application.Services;
using Gamification.Domain.Entities;
using Moq;

namespace Gamification.Application.Tests.Services;

public class RuleServiceTests
{
    private readonly Mock<IApplicationRepository> _appRepoMock;
    private readonly Mock<IRuleRepository> _ruleRepoMock;
    private readonly RuleService _service;

    public RuleServiceTests()
    {
        _appRepoMock = new Mock<IApplicationRepository>();
        _ruleRepoMock = new Mock<IRuleRepository>();
        _service = new RuleService(_appRepoMock.Object, _ruleRepoMock.Object);
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
            new RuleEntity { Id = 1, BadgeId = 10, NumberOfPoints = 50, OnEventType = "LOGIN" },
            new RuleEntity { Id = 2, BadgeId = null, NumberOfPoints = 20, OnEventType = "PURCHASE" }
        };

        _ruleRepoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(rules);

        // Act
        var result = await _service.GetAllAsync("key", "pass");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("LOGIN", result[0].OnEventType);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        // Act
        var result = await _service.GetAllAsync("bad", "creds");

        // Assert
        Assert.Empty(result);
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
            OnEventType = "LEVEL_UP"
        };

        _ruleRepoMock.Setup(r => r.GetByIdAsync(1, 5)).ReturnsAsync(rule);

        // Act
        var result = await _service.GetByIdAsync("key", "pass", 5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("LEVEL_UP", result!.OnEventType);
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
    public async Task GetByIdAsync_ShouldReturnNull_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        // Act
        var result = await _service.GetByIdAsync("bad", "creds", 1);

        // Assert
        Assert.Null(result);
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

        var dto = new CreateRuleDto
        {
            BadgeId = 10,
            NumberOfPoints = 50,
            OnEventType = "LOGIN"
        };

        // Act
        var result = await _service.CreateAsync("key", "pass", dto);

        // Assert
        Assert.Equal("created", result.Status);
        _ruleRepoMock.Verify(r => r.AddAsync(It.IsAny<RuleEntity>()), Times.Once);
        _ruleRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        var dto = new CreateRuleDto();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
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

        var dto = new UpdateRuleDto
        {
            BadgeId = 20,
            NumberOfPoints = 200,
            OnEventType = "PURCHASE"
        };

        // Act
        var result = await _service.UpdateAsync("key", "pass", 10, dto);

        // Assert
        Assert.True(result);
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
    public async Task UpdateAsync_ShouldReturnFalse_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        var dto = new UpdateRuleDto();

        // Act
        var result = await _service.UpdateAsync("bad", "creds", 1, dto);

        // Assert
        Assert.False(result);
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
    public async Task DeleteAsync_ShouldReturnFalse_WhenCredentialsInvalid()
    {
        // Arrange
        _appRepoMock.Setup(r => r.GetByCredentialsAsync("bad", "creds"))
                    .ReturnsAsync((ApplicationEntity?)null);

        // Act
        var result = await _service.DeleteAsync("bad", "creds", 1);

        // Assert
        Assert.False(result);
    }
}
