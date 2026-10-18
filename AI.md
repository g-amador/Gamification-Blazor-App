You are helping me continue Gamification Blazor App. A gamification engine that allows to manage players and give points and badges to players when some events raises, and integratation with other projects.

# PROJECT GITHUB repo
https://github.com/g-amador/Gamification-Blazor-App/tree/V1.0

# PROJECT ORGANIZATION
Directory structure:

```text
src/
├── Gamification.Domain
│   ├── Entities
│   └── Enums
│
├── Gamification.Application
│   ├── DTOs
│   ├── Interfaces
│   └── Services
│
├── Gamification.Infrastructure
│   ├── Data
│   ├── Repositories
│   └── DependencyInjection
│
├── Gamification.API
│   ├── Controllers
│   ├── Middleware
│   └── Models
│
├── Gamification.BlazorUI
│   ├── Pages
│   ├── Layout
│   └── Services
│
└── Gamification.Application.Tests
```

# STACK

.NET 10, ASP.NET Core, EF Core, SQL Server, Blazor WASM, xUnit, Moq.

# CURRENT ARCHITECTURE

```
API
 ↓
Application Services
 ↓
Repository Interfaces
 ↓
Infrastructure Repositories
 ↓
EF Core DbContext
 ↓
SQL Server / InMemory
```

Projects:
1. `Gamification.Domain`
2. `Gamification.Application`
3. `Gamification.Infrastructure`
4. `Gamification.API`
5. `Gamification.BlazorUI`
6. `Gamification.Application.Tests`

# CODING STANDARDS

Always follow these rules:

For comments use:

```C#
/// for XML comments
```

For:
- public classes
- public properties
- public methods
- interface methods

use:

```C#
// inline comments
```

inside method bodies when explaining business logic.

For service implementations:

```C#
// <inheritdoc />
```
must be used on public methods.

The full documentation should only exist on interfaces.

# DTO ORGANIZATION

DTOs are grouped by module:

```text
DTOs/
├─ Application/
├─ Player/
├─ Badge/
├─ Rule/
├─ Event/
└─ Leaderboard/
```

Each module uses:

```text
XxxBaseDto
CreateXxxDto
UpdateXxxDto
XxxListItemDto
XxxDetailsDto
XxxCreatedResponseDto
```

When Create and Update have identical fields:

```C#
CreateXxxDto : XxxBaseDto
UpdateXxxDto : XxxBaseDto
```

# IMPLEMENTED MODULES

## Applications

Features:

- Create application
- Get application
- Update application
- Delete application

Authentication:

```HTTP
apiKey
apiPassword
```

Application rules:

- apiKey is unique
- apiPassword is unique

## Players

Features:

- Create
- Get all
- Get by id
- Update
- Delete

Player fields:

```text
Id
FirstName
LastName
Email
NumberOfPoints
ApplicationId
```

Players belong to Applications.

## Badges

Features:

- Create
- Get all
- Get by id
- Update
- Delete

Fields:

```text
Id
Name
Description
Icon
ApplicationId
```

Badges belong to Applications.

## Rules

Features:

- Create
- Get all
- Get by id
- Update
- Delete

Rules determine what happens when an Event occurs.

Fields:

```text
Id
BadgeId (nullable)
NumberOfPoints
OnEventType
ApplicationId
```

Rules can:

- award points
- award badges
- do both

Many rules can exist for the same event type.

## Events

Features:

- Create
- Get all
- Get by id

Events cannot be updated.

Events cannot be deleted.

Fields:

```text
Id
PlayerId
Type
Timestamp
ApplicationId
```

Business logic:

When creating an Event:

1. Store the Event.
2. Find all Rules where:
```C#
rule.OnEventType == event.Type
```
3. Award all matching points.
4. Award any matching badges.
5. If the player already owns the badge:
```text
Do nothing.
```

## Leaderboard

Features:

```HTTP
GET /leaderboard
```

Returns:

```JSON
{
    "name": "",
    "description": "",
    "ranking": [
    {
        "playerId": 0,
        "points": 0
    }]
}
```

Rules:

1. Top 5 players
2. Ordered by NumberOfPoints descending

# CURRENT STATUS

The API is fully functional using:

```C#
UseInMemoryDatabase(...)
```
through EF Core.

Repository pattern is implemented.

Controllers exist for:

```text
Applications
Players
Badges
Rules
Events
Leaderboard
```

# Next Priorities

Before building UI or integrations:

1. Move from InMemory DB to local SQL Server.
2. Configure Entity Framework properly:
- Relationships
- Constraints
- Indexes
- SQL project
3. Finalize PlayerBadgeEntity.
4. Add Pagination.
5. Optimize repositories:
- GetTopPlayersAsync
- GetRulesByEventTypeAsync
6. Add FluentValidation.
7. Add ProblemDetails global exception handling.
8. Add Integration Tests.

# DOMAIN RELATIONSHIPS

```
Application
 ├── Players (1:N)
 ├── Badges (1:N)
 ├── Rules (1:N)
 └── Events (1:N)

Player
 ├── Events (1:N)
 └── Badges (M:N)

Badge
 └── Players (M:N)

Rule
 └── Badge (0:1)
 ```

# VALIDATION & ERROR HANDLING

- All Create and Update DTOs require FluentValidation validators.
- Validation belongs in Application layer.
- Controllers do not perform validation.
- Use ProblemDetails responses.
- Do not throw generic Exception.
- Use:
  - NotFoundException
  - ValidationException
  - ConflictException
  - UnauthorizedException
- Exception classes belong to Application.Exceptions.
- Global middleware handles exception translation.

# API STANDARDS

- Async everywhere.
- Controller actions, services, and repositories are asynchronous.
- Do not use `.Result`, `.Wait()` or `Task.Run()`.
- Prefer CancellationToken on new public methods.
- Standard method names:
  - `GetByIdAsync`
  - ``GetAllAsync`
  - `CreateAsync`
  - `UpdateAsync`
  - `DeleteAsync`
- Use `ActionResult<T>`.
- Use CreatedAtAction for POST endpoints.

# DATABASE

- Use SQL Server and EF Core Migrations.
- Configure entities using IEntityTypeConfiguration<T>.
- Do not place entity configuration in DbContext.
- Explicitly configure all foreign keys.
- Create indexes for:
  - Application.ApiKey
  - Application.ApiPassword
  - Player.Email
  - Rule.OnEventType
  - Event.Timestamp

# TESTING

Implemented:
- ApplicationServiceTests
- PlayerServiceTests
- BadgeServiceTests
- RuleServiceTests
- EventServiceTests
- LeaderboardServiceTests

Use:
- xUnit
- Moq
- Arrange / Act / Assert

Minimum coverage:
- Success
- Not found
- Validation failure
- Duplicate data
- Repository exception

Naming:
MethodName_Scenario_ExpectedResult

# DEVELOPMENT RULES

- Business logic belongs in Application Services.
- Controllers and Blazor pages contain no business logic.
- API layer never accesses DbContext directly.
- Repository interfaces live in Application.
- Repository implementations live in Infrastructure.
- DTOs never expose navigation properties.
- All new services and repositories must be registered in DI.
- Do not introduce MediatR, CQRS, or Vertical Slice architecture unless explicitly requested.
- Do not break existing API contracts.

# PAGINATION STANDARD

Paged requests use:

PageNumber
PageSize

Paged responses use:

```C#
PagedResult<T>
{
    Items
    TotalCount
    PageNumber
    PageSize
    TotalPages
}
```

# PERFORMANCE

Avoid loading complete tables into memory.

Filtering, sorting and pagination must be executed in SQL.

Use `AsNoTracking()` for read-only queries.

# CHANGE MANAGEMENT

Implement only the requested feature.

1. Do not refactor unrelated files.
2. Do not rename existing types.
3. Do not move files unless explicitly requested.
4. Prefer incremental improvements over large rewrites.

# AI INSTRUCTIONS

- Follow all documented architecture and conventions.
- Preserve existing API contracts.
- Provide complete production-ready code.
- Include tests and DI registration when required.
- Do not refactor unrelated files unless requested.
- Use explicit mappings. Do not use AutoMapper.
- API contracts must remain consumer-agnostic and support Web, Mobile, OutSystems and future integrations.

---

Please continue from this exact state and help me implement the next improvements using the same coding conventions and architecture.