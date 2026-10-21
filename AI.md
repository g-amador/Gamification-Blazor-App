# AI.md

You are helping me continue Gamification Blazor App. A gamification engine that allows to manage players and give points and badges to players when some events are raised, and integration with other projects.

> **Single source of truth.** This file describes the *real* current state, verified against the code.
> Sections are explicitly marked: `## STATE: IMPLEMENTED` or `## STATE: TARGET (not yet implemented)`.
> Do not assume a feature exists unless it is in an IMPLEMENTED section.

# PROJECT GITHUB REPO

https://github.com/g-amador/Gamification-Blazor-App (branch V1.0)

# PROJECT ORGANIZATION

Directory structure (as it exists today):

```text
src/
├── Gamification.Domain            # Entities + enums (no EF, no DTOs)
├── Gamification.Application       # DTOs, service + repository interfaces, services
├── Gamification.Infrastructure    # DbContext, repositories, EF/DI registration
├── Gamification.API               # Controllers (REST endpoints)
└── Gamification.BlazorUI          # Blazor WebAssembly UI (template shell)
└── Gamification.Application.Tests # xUnit + Moq service tests
```

The solution file is `src/GamificationSolution.slnx` — the XML-based `.slnx` format used in .NET 10 — and includes all 6 projects:

```bash
dotnet build src/GamificationSolution.slnx
dotnet test  src/GamificationSolution.slnx
```

# STACK

.NET 10, ASP.NET Core, EF Core, SQL Server (target), EF InMemory (current dev DB), Blazor WASM, xUnit, Moq.

# CURRENT ARCHITECTURE

```text
API
 ↓
Application Services
 ↓
Repository Interfaces (Application layer)
 ↓
Infrastructure Repositories
 ↓
EF Core DbContext
 ↓
InMemory (today) / SQL Server (tomorrow)
```

Projects:

1. `Gamification.Domain`
2. `Gamification.Application`
3. `Gamification.Infrastructure`
4. `Gamification.API`
5. `Gamification.BlazorUI`
6. `Gamification.Application.Tests`

# DATABASE STRATEGY

## STATE: IMPLEMENTED

- The API currently runs on **EF Core InMemory**:

```csharp
// src/Gamification.API/Program.cs
builder.Services.AddInfrastructure(
    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
    useInMemory: true
);
```

- `AddInfrastructure(connectionString, useInMemory)` in `Gamification.Infrastructure` already has both branches:
  - `useInMemory: true` → `UseInMemoryDatabase("GamificationDb")`
  - `useInMemory: false` → `UseSqlServer(connectionString)`

## STATE: TARGET (not yet implemented)

Migrations plan, in order:

1. **LocalDB first.** Run locally against **SQL Server LocalDB** (`(localdb)\MSSQLLocalDB`), configured via `DefaultConnection` in `appsettings.json` + EF Core Migrations. Flip `useInMemory: false`.
2. **TSQL database later.** The connection string is the swap point only: the same migrated schema runs unchanged against any full SQL Server / T-SQL instance.
3. **Keep the InMemory path alive.** Never remove `useInMemory` — it stays the default for quick start and is used for the fastest local boot.

Rules:

- Do not hardcode provider choice in services or repositories. Only the DI registration decides the provider.
- Repositories must not use provider-specific APIs (InMemory and SQL Server must behave identically).

# CODING STANDARDS

Always follow these rules:

For comments use:

```csharp
/// <summary>XML documentation comments</summary>
```

- XML `///` comments: public classes, public properties, public methods, interface methods.
- `//` inline comments: inside method bodies when explaining business logic.
- Service implementations use `<inheritdoc />` on public methods.
- Full documentation lives only on interfaces; implementations inherit it.

# DTO ORGANIZATION

## STATE: IMPLEMENTED

DTOs are grouped by module:

```text
DTOs/
├── Application/
├── Player/
├── Badge/
├── Rule/
├── Event/
└── Leaderboard/
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

```csharp
CreateXxxDto : XxxBaseDto
UpdateXxxDto : XxxBaseDto
```

Exception: `Leaderboard` uses only `LeaderboardItemDto` + `LeaderboardResponseDto` (read-only module).

# IMPLEMENTED MODULES

## Applications

Features:

- Create application (no auth required — this is the entry point)
- Get application (by credentials)
- Update application (full update, including `apiKey`/`apiPassword`)
- Delete application (cascades to all related resources)

Authentication:

Credentials are sent on **every request except POST /application** as HTTP headers:

```text
apiKey
apiPassword
```

Application rules:

- `apiKey` is unique
- `apiPassword` is unique
- Duplicate credentials are rejected on create/update

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

Players belong to Applications. A player is always scoped to one application.

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
- do both (two rules may also target the same event type)

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
Type (currently a string — enum introduction is a pending decision)
Timestamp
ApplicationId
```

Business logic (when creating an Event):

1. Resolve the application from credentials.
2. Store the Event.
3. Find all Rules of that application where `rule.OnEventType == event.Type`.
4. Award all matching points.
5. Award any matching badges; if the player already owns the badge, do nothing.

> Known debt (fix during the SQL Server migration): this operation currently calls
> `SaveChangesAsync()` on two repositories instead of one atomic save.

## Leaderboard

- `GET /leaderboard` with credentials in headers
- Top 5 players of the application
- Ordered by `NumberOfPoints` descending
- Returns application name/description + ranking of `playerId`/`points`

# API CONTRACT

Crucial: **every request carries the application credentials in the headers**, except:

- `POST /application` (creates the first application)
- Everything served from the landing page (`/`, `/scalar`, `/openapi/v1.json`)

Routes (verified from controllers):

| Resource    | Route         | Verbs                          | Auth |
| ----------- | ------------- | ------------------------------ | ---- |
| Application | `application` | POST, GET, PUT, DELETE         | Only POST |
| Players     | `players`     | GET (all), GET `{id}`, POST, PUT, DELETE | All |
| Badges      | `badges`      | GET (all), GET `{id}`, POST, PUT, DELETE | All |
| Rules       | `rules`       | GET (all), GET `{id}`, POST, PUT, DELETE | All |
| Events      | `events`      | GET (all), GET `{id}`, POST, PUT, DELETE | All (PUT/DELETE currently absent) |
| Leaderboard | `leaderboard` | GET                            | GET  |

Response shapes (current implementation):

- `GET` collection endpoints return `200 OK` with `{"items": [ ... ]}`
- `GET {id}` not found → `404`
- `POST` returns `201 Created` with a `XxxCreatedResponseDto` (`status`, `url`, `id`)
- `PUT` success → `200`, failure → `404` (resource modules) or `401` (application module)
- Leaderboard with invalid credentials → `404` (until standardization below)

## Auth status codes

- **Current behavior is inconsistent across controllers** (unauthorized requests yield 401, 404 or 200+empty depending on the module).
- **Target standard: invalid or missing credentials always → `401 Unauthorized`.** Implement while adding global exception handling; do not introduce new inconsistency.

# CURRENT STATUS

- API is fully functional on `UseInMemoryDatabase(...)` (EF Core).
- Repository pattern is implemented for Applications, Players, Badges, Rules, Events.
- There is **no `LeaderboardRepository`** — `LeaderboardService` composes `IApplicationRepository` + `IPlayerRepository`. Introduce it (or a query method) during the SQL Server/performance work.
- Controllers exist for: `Applications`, `Players`, `Badges`, `Rules`, `Events`, `Leaderboard`.
- API startup uses OpenAPI + Scalar (`/scalar` in Development) and a simple HTML landing page at `/`.
- `Gamification.BlazorUI` builds but is a **template shell**: `Home.razor`, `NotFound.razor`, `MainLayout.razor`, `wwwroot` assets, bare `HttpClient`. No API client services, no domain pages yet.

# NEXT PRIORITIES

Before building UI or integrations, in order:

1. Move from InMemory DB to **LocalDB** (see DATABASE STRATEGY): `appsettings.json` `DefaultConnection` + EF Core Migrations; flip `useInMemory: false` for a SQL run while keeping the InMemory option.
2. Configure Entity Framework properly:
   - Relationships
   - Constraints
   - Indexes
   - SQL project
3. Finalize the Player ↔ Badge join configuration (explicit join entity/table).
4. Standardize auth failures → `401` everywhere (see Auth status codes).
5. Single atomic save per service operation (replace double `SaveChangesAsync`).
6. Add pagination.
7. Optimize repositories:
   - `GetTopPlayersAsync` (top N by points, executed in SQL)
   - `GetRulesByEventTypeAsync`
8. Introduce FluentValidation (VALIDATION & ERROR HANDLING below).
9. Add ProblemDetails global exception handling + custom exception types.
10. Add integration tests.

# DOMAIN RELATIONSHIPS

```text
Application
 ├── Players (1:N)
 ├── Badges (1:N)
 ├── Rules  (1:N)
 └── Events (1:N)

Player
 ├── Events (1:N)
 └── Badges (M:N via implicit EF join — to be finalized as explicit configuration)

Badge
 └── Players (M:N)

Rule
 └── Badge (0:1)
```

Entity names in code use the `*Entity` suffix: `ApplicationEntity`, `PlayerEntity`, `BadgeEntity`, `RuleEntity`, `EventEntity`.

# VALIDATION & ERROR HANDLING

## STATE: TARGET (not yet implemented)

- All Create and Update DTOs get FluentValidation validators.
- Validation belongs in the Application layer; controllers do not perform validation.
- Use ProblemDetails responses via global exception middleware.
- Do not throw generic `Exception` types. Extract and throw:
  - `NotFoundException`
  - `ValidationException`
  - `ConflictException`
  - `UnauthorizedException`
- Exception classes belong to `Gamification.Application/Exceptions`.
- Global middleware translates exceptions to ProblemDetails + status codes.
- Until this lands, existing `InvalidOperationException` throws in services are the accepted placeholder — replace them when implementing this section, do not copy the pattern into new code.

# API STANDARDS

- Async everywhere; controllers, services, and repositories are asynchronous.
- Do not use `.Result`, `.Wait()` or `Task.Run()`.
- Prefer `CancellationToken` on new public methods (existing ones lack it — add when touching).
- Standard method names:
  - `GetByIdAsync`
  - `GetAllAsync`
  - `CreateAsync`
  - `UpdateAsync`
  - `DeleteAsync`
- Use `ActionResult<T>` / `IActionResult` for controller returns.
- POST endpoints return `201 Created` with the created resource URL.

# DATABASE (TARGET-STATE CONFIGURATION)

- SQL Server (LocalDB locally, full TSQL later) + EF Core Migrations.
- Configure entities using `IEntityTypeConfiguration<T>`; do not place configuration inside the DbContext.
- Explicitly configure all foreign keys.
- Create indexes for:
  - `Application.ApiKey`
  - `Application.ApiPassword`
  - `Player.Email`
  - `Rule.OnEventType`
  - `Event.Timestamp`

# TESTING

## STATE: IMPLEMENTED

- xUnit + Moq, repository mocks constructed per service.
- Test classes:
  - `ApplicationServiceTests`
  - `PlayerServiceTests`
  - `BadgeServiceTests`
  - `RuleServiceTests`
  - `EventServiceTests`
  - `LeaderboardServiceTests`
- Naming: `MethodName_Scenario_ExpectedResult`
- Structure: Arrange / Act / Assert, plus section comment headers per feature.

Minimum coverage for tests you add:

- Success
- Not found
- Validation failure
- Duplicate data
- Repository exception

Integration tests (STATE: TARGET) will use the real database provider via `WebApplicationFactory`.

# DEVELOPMENT RULES

- Business logic belongs in Application Services.
- Controllers and Blazor pages contain no business logic.
- API layer never accesses `DbContext` directly.
- Repository interfaces live in Application; implementations live in Infrastructure.
- DTOs never expose navigation properties.
- All new services and repositories must be registered in DI (currently `InfrastructureServiceRegistration`; split Application-layer registrations into an `AddApplication()` extension when touching DI).
- Do not introduce MediatR, CQRS, or Vertical Slice architecture unless explicitly requested.
- Do not break existing API contracts.

# PAGINATION STANDARD

## STATE: TARGET (not yet implemented — define before adding it)

Paged requests use:

```text
PageNumber
PageSize
```

Paged responses use:

```csharp
PagedResult<T>
{
    Items
    TotalCount
    PageNumber
    PageSize
    TotalPages
}
```

Do not paginate existing collection endpoints until the standard is introduced in one endpoint first (`players` is the candidate).

# PERFORMANCE

- Avoid loading complete tables into memory.
- Filtering, sorting and pagination must be executed in SQL.
- Use `AsNoTracking()` for read-only queries.
- Known violations to fix (tracked in NEXT PRIORITIES -8): leaderboard loads all players, event creation loads all rules of the application.

# CHANGE MANAGEMENT

Implement only the requested feature.

1. Do not refactor unrelated files.
2. Do not rename existing types.
3. Do not move files unless explicitly requested.
4. Prefer incremental improvements over large rewrites.
5. When code contradicts this document, flag it in the response before changing anything.

# AI INSTRUCTIONS

- Follow all documented architecture and conventions.
- Preserve existing API contracts.
- Provide complete production-ready code.
- Include tests and DI registration when required.
- Do not refactor unrelated files unless requested.
- Use explicit mappings. Do not use AutoMapper.
- API contracts must remain consumer-agnostic and support Web, Mobile, OutSystems and future integrations.
- When unsure between two behaviors, choose the one already present in the code over inventing a new pattern.

---

Please continue from this exact state and help me implement the next improvements using the same coding conventions and architecture.
