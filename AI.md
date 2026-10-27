# AI.md

You are helping me continue Gamification Blazor App. A gamification engine that allows to manage players and give points and badges to players when some events are raised, and integration with other projects.

> **Single source of truth.** This file describes the *real* current state, verified against the code.
> Sections are explicitly marked: `## STATE: IMPLEMENTED` or `## STATE: TARGET (not yet implemented)`.
> Do not assume a feature exists unless it is in an IMPLEMENTED section.
> The API contract is aligned with the Gitification blueprint (https://gitification.docs.apiary.io/#reference/applications)
> adapted to our flat routes and header authentication.

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
├── Gamification.BlazorUI          # Blazor WebAssembly UI (template shell)
└── Gamification.Application.Tests # xUnit + Moq service tests
```

The solution file is `src/GamificationSolution.slnx` — the XML-based `.slnx` format used in .NET 10 — and includes all 6 projects:

```bash
dotnet build src/GamificationSolution.slnx
dotnet test  src/GamificationSolution.slnx
```

# STACK

.NET 10, ASP.NET Core, EF Core, SQL Server / LocalDB (default dev DB), EF InMemory (optional, config flag), Blazor WASM, xUnit, Moq.

# CURRENT ARCHITECTURE

```text
API (controllers read apiKey/apiPassword headers)
 ↓
Application Services (business logic, rule evaluation)
 ↓
Repository Interfaces (Application layer)
 ↓
Infrastructure Repositories
 ↓
EF Core DbContext
 ↓
LocalDB by default / InMemory via `Database:UseInMemory` flag / full TSQL by connection string
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

- The API runs on **SQL Server LocalDB** by default, with EF Core Migrations applied at startup:

```json
// src/Gamification.API/appsettings.json
"ConnectionStrings": { "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=GamificationDb;..." },
"Database": { "UseInMemory": false }
```

```csharp
// src/Gamification.API/Program.cs
var useInMemory = builder.Configuration.GetValue("Database:UseInMemory", false);
builder.Services.AddInfrastructure(connectionString, useInMemory);
...
if (!useInMemory)
{
    // apply EF Core migrations at startup (db.Database.Migrate())
}
```

- `AddInfrastructure(connectionString, useInMemory)` in `Gamification.Infrastructure` has both branches:
  - `useInMemory: false` → `UseSqlServer(connectionString)` (default)
  - `useInMemory: true` → `UseInMemoryDatabase("GamificationDb")`
- Initial migration: `src/Gamification.Infrastructure/Data/Migrations/` (`20261025101441_InitialCreate`).
- Cascade matrix is configured in `GamificationDbContext.OnModelCreating` (SQL Server only allows one cascade path per table):
  - Applications cascade to Players / EventTypes / BadgeCategories / Badges / Rules.
  - Players cascade to Events (single cascade chain for events).
  - `Events → Application` and `Events/Rules → EventType` are `Restrict` (NO ACTION).
  - `Rules → Badge`, `Badges → Category` are `ClientSetNull` (optional refs).

## STATE: TARGET (not yet implemented)

1. **Full TSQL database.** The connection string is the swap point only: the same migrated schema runs unchanged against any full SQL Server / T-SQL instance.
2. **Keep the InMemory path alive** for tests/quick boot — flip `Database:UseInMemory` to `true`.

Rules:

- Do not hardcode provider choice in services or repositories. Only the DI registration decides the provider.
- Repositories must not use provider-specific APIs (InMemory and SQL Server must behave identically).

# CODING STANDARDS

Always follow these rules:

- **Central Package Management**: declare package versions only in the repo-root `Directory.Packages.props`; `PackageReference` elements in csproj files never carry a `Version` attribute. When adding a NuGet dependency, add a `PackageVersion` entry there and a versionless reference in the project.

For comments use:

```csharp
/// <summary>XML documentation comments</summary>
```

- XML `///` comments: public classes, public properties, public methods, interface methods.
- `//` inline comments: inside method bodies when explaining business logic.
- Service implementations use `<inheritdoc />` on public methods.
- Class-level remarks follow the Gitification blueprint wording (the resource description from
  https://gitification.docs.apiary.io) and must match between controller, service and interface.
- Full documentation lives only on interfaces; implementations inherit it.

# DTO ORGANIZATION

## STATE: IMPLEMENTED

DTOs are grouped by module:

```text
DTOs/
├── Application/
├── Player/
├── Badge/
├── BadgeCategory/
├── Rule/
├── Event/
├── EventType/
└── Leaderboard/
```

Each CRUD module uses:

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

Exceptions:

- `Leaderboard` uses only `LeaderboardItemDto` + `LeaderboardResponseDto` (read-only module).
- `Player` additionally has `PlayerBadgesDto` (badges of a player).
- `Application` uses `ApplicationListItemDto` for the get-all endpoint (no secrets exposed).

# IMPLEMENTED MODULES

## Applications

Features:

- Get all applications registered on the server (`GET /applications`, requires valid credentials; returns only public info)
- Create application (`POST /application`, no auth required — this is the entry point)
- Get own application by credentials (`GET /application`)
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
- Get badges of a player (`GET /players/{id}/badges`)
- Update (basic information only)
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
- Delete (links from rules are set to null)

Fields:

```text
Id
Name
Description
Icon
CategoryId (nullable)
ApplicationId
```

Badges belong to Applications. `categoryId` is optional per the Gitification blueprint.

## Badge Categories

Features (per Gitification blueprint — no delete):

- Create
- Get all
- Get by id
- Update

Fields:

```text
Id
Name
ApplicationId
```

Names are unique per application — duplicates are rejected.

## Rules

Features:

- Create
- Get all
- Get by id
- Update
- Delete (this doesn't change badges and points on past events)

Fields:

```text
Id
BadgeId (nullable)
NumberOfPoints
EventTypeId
ApplicationId
```

Rules can:

- award points
- award badges
- do both (two rules may also target the same event type)

## Events

Features (immutable resource — no update, no delete):

- Create
- Get all
- Get by id

Fields:

```text
Id
PlayerId
EventTypeId
Timestamp
ApplicationId
```

Business logic (when creating an Event):

1. Resolve the application from credentials.
2. Store the Event.
3. Find all Rules of that application where `rule.EventTypeId == event.EventTypeId`.
4. Award all matching points.
5. Award any matching badges; if the player already owns the badge, do nothing.

> Implemented: this operation uses a **single atomic save** — one `SaveChangesAsync()`
> on the event repository persists both the event insert and the points update
> (verified as one SQL batch: `INSERT INTO [Events]` + `UPDATE [Players]`).

> Known debt: badge awarding still uses the legacy `player.Badges.Add(new BadgeEntity { Id = rule.BadgeId.Value })`
> hack. On a real SQL Server database EF will try to INSERT a badge row with an explicit PK —
> rework the Player ↔ Badge allocation (see NEXT PRIORITIES) before relying on badge rules in production.

## Event Types

Features (per Gitification blueprint — no delete):

- Create
- Get all
- Get by id
- Update

Fields:

```text
Id
Name
ApplicationId
```

Names are unique per application — duplicates are rejected.

> Done: events (`EventEntity.EventTypeId`) and rules (`RuleEntity.EventTypeId`) now reference
> `EventTypeEntity` by id; responses expose `eventTypeId` + read-convenience `eventTypeName`.
> Inputs accept `eventTypeId` only — the free-string contract is gone.

## Leaderboard

- `GET /leaderboard` with credentials in headers
- Top 5 players of the application
- Ordered by `NumberOfPoints` descending
- Returns application name/description + ranking of `playerId`/`points`

# API CONTRACT

Every request carries the application credentials in the headers, except:

- `POST /application` (creates the first application)
- Everything served from the landing page (`/`, `/scalar`, `/openapi/v1.json`)

Routes (verified from controllers):

| Resource    | Route             | Verbs                                        | Auth |
| ----------- | ----------------- | -------------------------------------------- | ---- |
| Application | `application`     | POST, GET, PUT, DELETE                       | Only POST |
| Applications| `applications`    | GET (all registered apps, public info only)  | GET |
| Players     | `players`         | GET (all), GET `{id}`, GET `{id}/badges`, POST, PUT, DELETE | All |
| Badges      | `badges`          | GET (all), GET `{id}`, POST, PUT, DELETE     | All |
| Badge Categories | `badge-categories` | GET (all), GET `{id}`, POST, PUT       | All |
| Rules       | `rules`           | GET (all), GET `{id}`, POST, PUT, DELETE     | All |
| Events      | `events`          | GET (all), GET `{id}`, POST                  | All |
| Event Types | `event-types`     | GET (all), GET `{id}`, POST, PUT             | All |
| Leaderboard | `leaderboard`     | GET                                          | GET |

Response shapes:

- `GET` collection endpoints return `200 OK` with `{"items": [ ... ]}`
- `GET {id}` not found → `404`
- `POST` returns `201 Created` with a `XxxCreatedResponseDto` (`status`, `url`, `id`)
- `PUT` success → `200`, failure (resource not found) → `404`
- `DELETE` success → `200`, not found → `404`
- Invalid or missing credentials → `401 Unauthorized` everywhere (implemented below)

## Auth status codes

## STATE: IMPLEMENTED

- Invalid or missing `apiKey`/`apiPassword` → `401 Unauthorized` **on every endpoint**.
- Services throw `UnauthorizedException` (`Gamification.Application/Exceptions`) when
  `GetByCredentialsAsync` finds no matching application.
- `GlobalExceptionHandler` middleware (`src/Gamification.API/Middleware/`) translates it into a
  `401` response with `Content-Type: application/problem+json` (ProblemDetails).
- Non-auth failures (entity not found) still return `null`/`false` from services → controllers respond `404`.

The Postman collection (`Gamification API.postman_collection.json`) contains one request per
REST method above with response examples; keep it in sync when the contract changes.

# CURRENT STATUS

- API is fully functional on **SQL Server LocalDB** (`Database:UseInMemory` = `false` by default), with migrations applied at startup; the InMemory path remains available via config.
- Repository pattern is implemented for all modules (Application, Player, Badge, BadgeCategory, EventType, Rule, Event).
- There is **no `LeaderboardRepository`** — `LeaderboardService` composes `IApplicationRepository` + `IPlayerRepository`.
- Controllers exist for: `Applications`, `ApplicationsList`, `Players`, `Badges`, `BadgeCategories`, `EventTypes`, `Rules`, `Events`, `Leaderboard`.
- The XML documentation of controllers, services and interfaces follows the Gitification blueprint (docs.apiary.io) wording.
- API startup uses OpenAPI + Scalar (`/scalar` in Development) and a simple HTML landing page at `/`.
- Invalid credentials → `401 ProblemDetails` on every endpoint via `GlobalExceptionHandler` middleware.
- `Gamification.BlazorUI` builds but is a **template shell**: `Home.razor`, `NotFound.razor`, `MainLayout.razor`, `wwwroot` assets, bare `HttpClient`. No API client services, no domain pages yet.
- Unit tests: 84 passing (Application, Player, Badge, BadgeCategory, EventType, Rule, Event, Leaderboard services).
- Postman collection kept in sync: `Gamification API.postman_collection.json` (33 requests / 8 folders).

# NEXT PRIORITIES

Before building UI or integrations, in order:

1. Finalize the Player ↔ Badge allocation (rework the `player.Badges.Add(new BadgeEntity { Id = … })` legacy hack that breaks on SQL Server; explicit join entity/table).
2. Configure EF properly beyond the initial migration:
   - Unique indexes for EventType and BadgeCategory names — the duplicate check currently races (checked in code, not enforced in schema)
   - Indexes (`Application.ApiKey`, `Player.Email`, `Event.Timestamp`)
   - Move config to `IEntityTypeConfiguration<T>` (currently inline in `OnModelCreating`)
3. Add unique-name conflicts as `ConflictException` (currently `InvalidOperationException`) — see VALIDATION & ERROR HANDLING.
4. Add pagination.
5. Optimize repositories:
   - `GetTopPlayersAsync` (top N by points, executed in SQL)
   - `GetRulesByEventTypeAsync`
6. Introduce FluentValidation (VALIDATION & ERROR HANDLING below).
7. Add integration tests (WebApplicationFactory against LocalDB).

# DOMAIN RELATIONSHIPS

```text
Application
 ├── Players (1:N)
 ├── Badges (1:N)
 ├── BadgeCategories (1:N)
 ├── EventTypes (1:N)
 ├── Rules (:N)
 └── Events (1:N)

Player
 ├── Events (1:N)
 └── Badges (M:N via implicit EF join — to be finalized as explicit configuration)

Badge
 └── Category (0:1, BadgeCategory)

Rule
 └── Badge (0:1)
```

Entity names in code use the `*Entity` suffix: `ApplicationEntity`, `PlayerEntity`, `BadgeEntity`, `BadgeCategoryEntity`, `EventTypeEntity`, `RuleEntity`, `EventEntity`.

# VALIDATION & ERROR HANDLING

## STATE: PARTIALLY IMPLEMENTED

- `UnauthorizedException` + `GlobalExceptionHandler` middleware (401 ProblemDetails) are implemented — see Auth status codes.
- Still TARGET:
  - All Create and Update DTOs get FluentValidation validators.
  - `NotFoundException`, `ValidationException`, `ConflictException` (for duplicate apiKey/apiPassword/event type name/category name) — the not-found path still relies on null/false service returns, and duplicate-name checks throw `InvalidOperationException`.
- Validation belongs in the Application layer; controllers do not perform validation.
- Do not throw generic `Exception` types for new code.
- Exception classes belong to `Gamification.Application/Exceptions`; global middleware translates them to ProblemDetails + status codes.

# API STANDARDS

- OpenAPI tags are sorted alphabetically via a document transformer in `Program.cs` (`AddOpenApi`), so Scalar groups render in stable alphabetical order. The HTML landing page at `/` is excluded from the description (`ExcludeFromDescription`).

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
  - `Rule.EventTypeId`
  - `Event.Timestamp`
  - `EventType (ApplicationId, Name)` — unique
  - `BadgeCategory (ApplicationId, Name)` — unique

# TESTING

## STATE: IMPLEMENTED

- xUnit + Moq, repository mocks constructed per service.
- Test classes (one per service):
  - `ApplicationServiceTests`
  - `PlayerServiceTests`
  - `BadgeServiceTests`
  - `BadgeCategoryServiceTests`
  - `EventTypeServiceTests`
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
- Do not break existing API contracts; additive JSON fields (like the badge `categoryId`) are allowed.
- Do not add DELETE endpoints for modules the Gitification blueprint leaves without delete (Badge Categories, Event Types) unless explicitly requested.

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
- Known violations to fix (tracked in NEXT PRIORITIES): leaderboard loads all players, event creation loads all rules of the application.

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
