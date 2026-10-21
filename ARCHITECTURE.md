# 📂 Project Architecture

> **Conventions and rules live in [AI.md](AI.md) — that file is the single source of truth for how to develop this project.**
> This file describes the structure of the code **as it actually exists**, verified against the repository.
> Planned-but-missing pieces are marked *(planned)*.

## Solution

The solution is `src/GamificationSolution.slnx` — the new XML-based solution format introduced for .NET (`dotnet sln` supports it in .NET 10; Visual Studio 2022 17.10+). It includes all 6 projects:

```bash
dotnet build src/GamificationSolution.slnx
dotnet test  src/GamificationSolution.slnx
```

Individual projects can still be built directly for faster iteration.

## Layering

```text
Gamification.API ──────► Gamification.Infrastructure ──────► Gamification.Application ──────► Gamification.Domain
   (HTTP, DTO mapping           (DbContext, EF providers,            (DTOs, service +                    (entities only,
    in controllers)              repository implementations)          repository interfaces,             no behavior, no EF)
                                                                      service implementations)
```

- `Gamification.API` and `Gamification.BlazorUI` reference `Gamification.Application`; the API also references `Gamification.Infrastructure` for DI wiring only.
- `Gamification.Domain` references nothing.

## Directory structure

```text
Gamification-Blazor-App/
├── AI.md                                        # Development rules, conventions, current state
├── ARCHITECTURE.md                              # This file: physical structure
├── Gamification API.postman_collection.json     # Postman collection for the API
├── LICENSE
├── README.md
└── src/
    ├── GamificationSolution.slnx                 # .slnx (XML) solution — all 6 projects
    ├── Gamification.Domain/                     # Domain layer: entities and enums (no EF, no DTOs)
    │   ├── Entities/
    │   │   ├── ApplicationEntity.cs             # Application (has unique apiKey + apiPassword)
    │   │   ├── PlayerEntity.cs                  # Player (belongs to Application; M:N with Badges)
    │   │   ├── BadgeEntity.cs                   # Badge (belongs to Application)
    │   │   ├── RuleEntity.cs                    # Rule (0:1 Badge; awarding definition per event type)
    │   │   └── EventEntity.cs                   # Event (P player + type + timestamp; immutable)
    │   └── Gamification.Domain.csproj
    │
    ├── Gamification.Application/                # Application layer: use cases
    │   ├── DTOs/                                # Request/response contracts grouped by module
    │   │   ├── Application/                     # Base, Create, Update, ListItem-free, CreatedResponse, ApplicationDto
    │   │   ├── Player/                          # Base, Create, Update, ListItem, Details, CreatedResponse, PlayerDto
    │   │   ├── Badge/                           # Base, Create, Update, ListItem, CreatedResponse, BadgeDto
    │   │   ├── Rule/                            # Base, Create, Update, ListItem, Details, CreatedResponse
    │   │   ├── Event/                           # Base, Create, ListItem, Details, CreatedResponse
    │   │   └── Leaderboard/                     # Item, Response (read-only module)
    │   ├── Interfaces/
    │   │   ├── IApplicationRepository.cs        # incl. GetByCredentialsAsync, ApiKey/PasswordExists
    │   │   ├── IPlayerRepository.cs
    │   │   ├── IBadgeRepository.cs
    │   │   ├── IRuleRepository.cs
    │   │   ├── IEventRepository.cs
    │   │   ├── IApplicationService.cs
    │   │   ├── IPlayerService.cs
    │   │   ├── IBadgeService.cs
    │   │   ├── IRuleService.cs
    │   │   ├── IEventService.cs
    │   │   └── ILeaderboardService.cs
    │   ├── Services/                            # Business logic (implement the interfaces above)
    │   │   ├── ApplicationService.cs
    │   │   ├── PlayerService.cs
    │   │   ├── BadgeService.cs
    │   │   ├── RuleService.cs
    │   │   ├── EventService.cs                  # Event creation + rule evaluation (points/badges)
    │   │   └── LeaderboardService.cs            # Composes Application + Player repos (no LeaderboardRepository)
    │   ├── Exceptions/                          # NotFoundException, ValidationException, ... (planned — see AI.md)
    │   └── Mappings/                            # (planned: explicit mapper classes; NO AutoMapper)
    │                                            # Mapping is currently inline in services
    │
    ├── Gamification.Infrastructure/             # Persistence
    │   ├── Data/
    │   │   ├── GamificationDbContext.cs         # DbSets: Applications, Badges, Events, Players, Rules
    │   │   ├── Configurations/                  # IEntityTypeConfiguration<T> files (planned)
    │   │   └── Migrations/                      # EF Core migrations (planned — SQL Server/LocalDB)
    │   ├── Repositories/
    │   │   ├── ApplicationRepository.cs         # incl. GetByCredentialsAsync, duplicate-key checks
    │   │   ├── PlayerRepository.cs              # GetByIdAsync Includes Badges
    │   │   ├── BadgeRepository.cs
    │   │   ├── RuleRepository.cs
    │   │   └── EventRepository.cs
    │   └── DependencyInjection/
    │       └── InfrastructureServiceRegistration.cs
    │                                            # AddInfrastructure(connectionString, useInMemory) →
    │                                            #   InMemory OR SqlServer + repo/service registrations
    │
    ├── Gamification.API/                        # REST API (consumer-agnostic: Web, Mobile, OutSystems)
    │   ├── Controllers/
    │   │   ├── ApplicationsController.cs        # Route "application" (singular); credentials via headers
    │   │   ├── PlayersController.cs             # Route "players"
    │   │   ├── BadgesController.cs              # Route "badges"
    │   │   ├── RulesController.cs               # Route "rules"
    │   │   ├── EventsController.cs              # Route "events"
    │   │   └── LeaderboardController.cs         # Route "leaderboard"
    │   ├── Middleware/
    │   │   └── GlobalExceptionHandler.cs        # ProblemDetails translation (planned — see AI.md)
    │   └── Program.cs
    │                                            # OpenAPI + Scalar (Dev), HTML landing page at "/",
    │                                            # AddInfrastructure(..., useInMemory: true) ← swap point
    │
    └── Gamification.BlazorUI/                   # Blazor WebAssembly UI — TEMPLATE SHELL
        ├── wwwroot/                             # Static assets
        │   ├── css/app.css                      # Global CSS
        │   └── index.html                       # Root HTML page
        ├── Layout/
        │   └── MainLayout.razor                 # Main layout
        ├── Pages/
        │   ├── Home.razor                       # Home page
        │   └── NotFound.razor                   # 404 page
        ├── Services/                            # HttpClient API wrappers (planned)
        ├── _Imports.razor
        ├── App.razor
        └── Program.cs                           # WASM bootstrap; bare HttpClient, no API wiring yet
    │
    └── Gamification.Application.Tests/          # Unit tests (xUnit + Moq)
        └── Services/
            ├── ApplicationServiceTests.cs
            ├── PlayerServiceTests.cs
            ├── BadgeServiceTests.cs
            ├── RuleServiceTests.cs
            ├── EventServiceTests.cs
            └── LeaderboardServiceTests.cs
```

## Database strategy (summary — full version in AI.md)

```text
Today:  InMemory        useInMemory: true   → UseInMemoryDatabase("GamificationDb")
Next:   LocalDB         useInMemory: false  → UseSqlServer("(localdb)\MSSQLLocalDB") + Migrations
Later:  Full TSQL       same code,          → swap DefaultConnection string
        (prod-instance)   migrations only
```

The provider choice is made **only** in `InfrastructureServiceRegistration.AddInfrastructure(connectionString, useInMemory)`. Services and repositories never see EF providers or connection strings directly.

## Key decisions already made

- Repository pattern, interfaces in Application, implementations in Infrastructure.
- Explicit mapping in services; **no AutoMapper**.
- Credentials (`apiKey`, `apiPassword`) travel as HTTP request headers on every endpoint except `POST /application`.
- Entities are pragmatic: POCOs with EF navigation properties (`*Entity` suffix), behavior lives in Application services.
- No MediatR, no CQRS, no vertical slices (by design — see AI.md).
