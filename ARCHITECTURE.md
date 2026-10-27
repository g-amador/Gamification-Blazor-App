# 📂 Project Architecture

> **Conventions and rules live in [AI.md](AI.md) — that file is the single source of truth for how to develop this project.**
> This file describes the structure of the code **at the file level**, as it actually exists in the repository.
> Planned-but-missing pieces are marked *(planned)*.

## Solution

The solution is `src/GamificationSolution.slnx` — the XML-based solution format introduced for .NET (`dotnet sln` supports it in .NET 10; Visual Studio 2022 17.10+). It includes all 6 projects:

```bash
dotnet build src/GamificationSolution.slnx
dotnet test  src/GamificationSolution.slnx
```

Individual projects can still be built directly for faster iteration.

## Layering

```text
Gamification.API ───────► Gamification.Infrastructure ───────► Gamification.Application ───────► Gamification.Domain
   (HTTP, header credentials,    (DbContext, EF providers,             (DTOs, service +                    (entities only,
    controller actions)           repository implementations)           repository interfaces,             no behavior, no EF)
                                                                        service implementations)
```

- `Gamification.API` references `Gamification.Application` + `Gamification.Infrastructure` (DI wiring only).
- `Gamification.BlazorUI` references nothing but the framework yet (standalone WASM shell).
- `Gamification.Domain` references nothing.
- Authentication: each controller extracts `apiKey` / `apiPassword` from the HTTP headers and passes them to its service; services resolve the application via `IApplicationRepository.GetByCredentialsAsync` and scope every resource to it.

## Directory structure

```text
Gamification-Blazor-App/
├── AI.md                                        # Development rules, conventions, current state
├── ARCHITECTURE.md                              # This file: physical structure (file level)
├── Gamification API.postman_collection.json     # Postman collection: one call per REST method
├── LICENSE
├── README.md
└── src/
    ├── GamificationSolution.slnx                # .slnx (XML) solution — all 6 projects
    │
    ├── Gamification.Domain/                     # Domain layer: entities and enums (no EF, no DTOs)
    │   ├── Entities/
    │   │   ├── ApplicationEntity.cs             # Application (unique apiKey + apiPassword)
    │   │   ├── PlayerEntity.cs                  # Player (belongs to Application; M:N with Badges)
    │   │   ├── BadgeEntity.cs                   # Badge (belongs to Application; optional CategoryId)
    │   │   ├── BadgeCategoryEntity.cs           # Category grouping badges (name unique per application)
    │   │   ├── RuleEntity.cs                    # Rule (0:1 Badge; awards points and/or badge per event type)
    │   │   ├── EventEntity.cs                   # Event (player + type + timestamp; immutable)
    │   │   └── EventTypeEntity.cs               # Event type (name, unique per application)
    │   └── Gamification.Domain.csproj
    │
    ├── Gamification.Application/                # Application layer: use cases
    │   ├── DTOs/                                # Request/response contracts grouped by module
    │   │   ├── Application/                     # Base, Create, Update, ListItem, CreatedResponse, ApplicationDto
    │   │   ├── Player/                          # Base, Create, Update, ListItem, PlayerDto, PlayerBadgesDto, CreatedResponse
    │   │   ├── Badge/                           # Base, Create, Update, ListItem, BadgeDto, CreatedResponse
    │   │   ├── BadgeCategory/                   # Base, Create, Update, ListItem, Details, CreatedResponse
    │   │   ├── Rule/                            # Base, Create, Update, ListItem, Details, CreatedResponse
    │   │   ├── Event/                           # Base, Create, ListItem, Details, CreatedResponse
    │   │   ├── EventType/                       # Base, Create, Update, ListItem, Details, CreatedResponse
    │   │   └── Leaderboard/                     # LeaderboardItemDto, LeaderboardResponseDto (read-only)
    │   ├── Interfaces/
    │   │   ├── IApplicationRepository.cs        # GetAllAsync, GetByCredentialsAsync, duplicate-key checks
    │   │   ├── IPlayerRepository.cs
    │   │   ├── IBadgeRepository.cs
    │   │   ├── IBadgeCategoryRepository.cs      # incl. NameExistsAsync
    │   │   ├── IEventTypeRepository.cs          # incl. NameExistsAsync
    │   │   ├── IRuleRepository.cs
    │   │   ├── IEventRepository.cs
    │   │   ├── IApplicationService.cs           # incl. GetAllAsync (all applications registered)
    │   │   ├── IPlayerService.cs                # incl. GetBadgesAsync (badges of a player)
    │   │   ├── IBadgeService.cs
    │   │   ├── IBadgeCategoryService.cs
    │   │   ├── IEventTypeService.cs
    │   │   ├── IRuleService.cs
    │   │   ├── IEventService.cs
    │   │   └── ILeaderboardService.cs
    │   ├── Services/                            # Business logic (implement the interfaces above)
    │   │   ├── ApplicationService.cs            # Includes GetAllAsync for the applications list
    │   │   ├── PlayerService.cs                 # Includes GetBadgesAsync
    │   │   ├── BadgeService.cs                  # Maps optional CategoryId
    │   │   ├── BadgeCategoryService.cs          # Name uniqueness per application
    │   │   ├── EventTypeService.cs              # Name uniqueness per application
    │   │   ├── RuleService.cs
    │   │   ├── EventService.cs                  # Event creation + rule evaluation (points/badges)
    │   │   └── LeaderboardService.cs            # Composes Application + Player repos (no LeaderboardRepository)
    │   ├── Exceptions/                          # NotFoundException, ValidationException, ... (planned — see AI.md)
    │   └── Mappings/                            # (planned: explicit mapper classes; NO AutoMapper)
    │                                            # Mapping is currently inline in services
    │
    ├── Gamification.Infrastructure/             # Persistence
    │   ├── Data/
    │   │   ├── GamificationDbContext.cs         # DbSets: Applications, Badges, BadgeCategories, Events,
    │   │   │                                    #         EventTypes, Players, Rules
    │   │   ├── Configurations/                  # IEntityTypeConfiguration<T> files (planned)
    │   │   └── Migrations/                      # EF Core migrations (planned — LocalDB/TSQL)
    │   ├── Repositories/
    │   │   ├── ApplicationRepository.cs         # incl. GetAllAsync, GetByCredentialsAsync
    │   │   ├── PlayerRepository.cs              # GetByIdAsync Includes the player's Badges
    │   │   ├── BadgeRepository.cs
    │   │   ├── BadgeCategoryRepository.cs       # incl. NameExistsAsync
    │   │   ├── EventTypeRepository.cs           # incl. NameExistsAsync
    │   │   ├── RuleRepository.cs
    │   │   └── EventRepository.cs
    │   └── DependencyInjection/
    │       └── InfrastructureServiceRegistration.cs
    │                                            # AddInfrastructure(connectionString, useInMemory) →
    │                                            #   InMemory OR SqlServer; registers all repositories + services
    │
    ├── Gamification.API/                        # REST API (consumer-agnostic: Web, Mobile, OutSystems)
    │   ├── Controllers/
    │   │   ├── ApplicationsListController.cs    # Route "applications" — GET: all applications registered
    │   │   ├── ApplicationsController.cs        # Route "application" — POST/GET/PUT/DELETE own application
    │   │   ├── PlayersController.cs             # Route "players" — CRUD + GET {id}/badges
    │   │   ├── BadgesController.cs              # Route "badges" — CRUD (delete nulls rule badges)
    │   │   ├── BadgeCategoriesController.cs     # Route "badge-categories" — GET/GET {id}/POST/PUT
    │   │   ├── RulesController.cs               # Route "rules" — CRUD
    │   │   ├── EventsController.cs              # Route "events" — GET list/GET {id}/POST (immutable resource)
    │   │   ├── EventTypesController.cs          # Route "event-types" — GET/GET {id}/POST/PUT
    │   │   └── LeaderboardController.cs         # Route "leaderboard" — GET
    │   ├── Middleware/
    │   │   └── GlobalExceptionHandler.cs        # ProblemDetails translation (planned — see AI.md)
    │   └── Program.cs
    │                                            # OpenAPI + Scalar (Dev), HTML landing page at "/",
    │                                            # AddInfrastructure(..., useInMemory: true) ← DB selector
    │
    └── Gamification.Application.Tests/          # Unit tests (xUnit + Moq, ARRANGE/ACT/ASSERT)
        └── Services/
            ├── ApplicationServiceTests.cs
            ├── PlayerServiceTests.cs
            ├── BadgeServiceTests.cs
            ├── BadgeCategoryServiceTests.cs
            ├── EventTypeServiceTests.cs
            ├── RuleServiceTests.cs
            ├── EventServiceTests.cs
            └── LeaderboardServiceTests.cs
```

## Endpoint matrix

| Resource | Route | GET list | GET {id} | POST | PUT | DELETE |
| --- | --- | --- | --- | --- | --- | --- |
| Applications (own) | `application` | GET = own app | – (GET by credentials) | ✔ | ✔ | ✔ |
| Applications (all) | `applications` | ✔ | – | – | – | – |
| Players | `players` | ✔ | ✔ | ✔ | ✔ | ✔ + `GET {id}/badges` |
| Badges | `badges` | ✔ | ✔ | ✔ | ✔ | ✔ |
| Badge Categories | `badge-categories` | ✔ | ✔ | ✔ | ✔ | – |
| Rules | `rules` | ✔ | ✔ | ✔ | ✔ | ✔ |
| Events | `events` | ✔ | ✔ | ✔ | – (immutable) | – (immutable) |
| Event Types | `event-types` | ✔ | ✔ | ✔ | ✔ | – |
| Leaderboard | `leaderboard` | ✔ | – | – | – | – |

POST endpoints return `201 Created` with `{ "status": "created", "url": "/resource/{id}", "id": n }`.
Collection endpoints return `200 OK` with `{"items": [ ... ]}`. GET by id returns the details DTO or `404`.

## Database strategy (summary — full version in AI.md)

```text
Default: LocalDB        UseInMemory=false → UseSqlServer("(localdb)\MSSQLLocalDB") + Migrations at startup
Optional: InMemory      UseInMemory=true  → UseInMemoryDatabase("GamificationDb")
Later:    Full TSQL     same code,        → swap DefaultConnection string
          (prod-instance) migrations only
```

The provider choice is made **only** in `InfrastructureServiceRegistration.AddInfrastructure(connectionString, useInMemory)`. Services and repositories never see EF providers or connection strings directly.

## Key decisions already made

- Repository pattern: interfaces in Application, implementations in Infrastructure.
- **Central Package Management** (`Directory.Packages.props` at repo root): `ManagePackageVersionsCentrally` enabled; all csproj `PackageReference` elements are versionless and versions are declared once, centrally. Add new packages to `Directory.Packages.props` first.
- Explicit mapping in services; **no AutoMapper**.
- Credentials (`apiKey`, `apiPassword`) travel as HTTP request headers on every endpoint except `POST /application`; every resource is scoped to the authenticated application. Invalid or missing credentials throw `UnauthorizedException`, translated by the `GlobalExceptionHandler` middleware into `401` ProblemDetails (`application/problem+json`) on every endpoint.
- Names of event types and badge categories are unique per application (enforced in services).
- Entities are pragmatic: POCOs with EF navigation properties (`*Entity` suffix); behavior lives in the Application services.
- No MediatR, no CQRS, no vertical slices (by design — see AI.md).
