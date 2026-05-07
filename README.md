# Gamification Blazor APP
Gamification Blazor APP to integrate in other projects. Allows to manage players and give points and badges to players when some events raises.

## Project structure

```
GamificationSolution/
│
├── Gamification.Domain/                                 # Domain layer: pure business logic
│   ├── Entities/                                        # Domain entities (no EF, no DTOs)
│   │   ├── Application.cs                               # Application entity
│   │   ├── Player.cs                                    # Player entity
│   │   ├── Badge.cs                                     # Badge entity
│   │   ├── Rule.cs                                      # Rule entity
│   │   └── Event.cs                                     # Event entity
│   │
│   └── Enums/
│       └── EventType.cs                                 # Enum for event types
│
├── Gamification.Application/                            # Application layer: DTOs, services, interfaces
│   ├── DTOs/                                            # Shared data contracts
│   │   ├── ApplicationDto.cs                            # DTO for Application
│   │   ├── PlayerDto.cs                                 # DTO for Player
│   │   ├── BadgeDto.cs                                  # DTO for Badge
│   │   ├── RuleDto.cs                                   # DTO for Rule
│   │   ├── EventDto.cs                                  # DTO for Event
│   │   └── LeaderboardDto.cs                            # DTO for Leaderboard
│   │
│   ├── Interfaces/                                      # Service interfaces
│   │   ├── IApplicationService.cs                       # Application service contract
│   │   ├── IPlayerService.cs                            # Player service contract
│   │   ├── IBadgeService.cs                             # Badge service contract
│   │   ├── IRuleService.cs                              # Rule service contract
│   │   ├── IEventService.cs                             # Event service contract
│   │   └── ILeaderboardService.cs                       # Leaderboard service contract
│   │
│   ├── Services/                                        # Business logic implementations
│   │   ├── ApplicationService.cs                        # Application logic
│   │   ├── PlayerService.cs                             # Player logic
│   │   ├── BadgeService.cs                              # Badge logic
│   │   ├── RuleService.cs                               # Rule logic
│   │   ├── EventService.cs                              # Event logic
│   │   └── LeaderboardService.cs                        # Leaderboard logic
│   │
│   └── Mappings/
│       └── AutoMapperProfiles.cs                        # AutoMapper configuration
│
├── Gamification.Infrastructure/                         # Infrastructure: EF Core, repositories, persistence
│   ├── Data/
│   │   ├── GamificationDbContext.cs                     # EF Core DbContext
│   │   └── SeedData.cs                                  # Initial seed data
│   │
│   ├── Repositories/                                    # Repository implementations
│   │   ├── ApplicationRepository.cs                     # Application repository
│   │   ├── PlayerRepository.cs                          # Player repository
│   │   ├── BadgeRepository.cs                           # Badge repository
│   │   ├── RuleRepository.cs                            # Rule repository
│   │   ├── EventRepository.cs                           # Event repository
│   │   └── LeaderboardRepository.cs                     # Leaderboard repository
│   │
│   └── DependencyInjection/
│       └── InfrastructureServiceRegistration.cs         # Registers infrastructure services
│
├── Gamification.API/                                    # Public API for Blazor + Mobile
│   ├── Controllers/                                     # REST endpoints
│   │   ├── ApplicationsController.cs                    # CRUD for Applications
│   │   ├── PlayersController.cs                         # CRUD for Players
│   │   ├── BadgesController.cs                          # CRUD for Badges
│   │   ├── RulesController.cs                           # CRUD for Rules
│   │   ├── EventsController.cs                          # CRUD for Events
│   │   └── LeaderboardController.cs                     # Leaderboard endpoint
│   │
│   ├── Filters/
│   │   └── ApiKeyAuthFilter.cs                          # API key validation filter
│   │
│   ├── Middleware/
│   │   └── ApiKeyMiddleware.cs                          # API key middleware
│   │
│   ├── Models/
│   │   └── ApiResponse.cs                               # Standard API response wrapper
│   │
│   └── Program.cs                                       # API startup, Swagger, DI, EF config
│
└── Gamification.BlazorUI/                               # Blazor WebAssembly UI
    ├── wwwroot/                                         # Static assets
    │   ├── css/
    │   │   └── app.css                                  # Global CSS
    │   ├── icon-192.png                                 # PWA icon
    │   └── index.html                                   # Root HTML page
    │
    ├── Layout/
    │   └── MainLayout.razor                             # Main layout for UI
    │
    ├── Pages/                                           # UI pages grouped by domain
    │   ├── Applications/
    │   │   ├── ApplicationList.razor                    # List applications
    │   │   ├── ApplicationDetail.razor                  # Application details
    │   │   └── ApplicationForm.razor                    # Create/edit application
    │   │
    │   ├── Players/
    │   │   ├── PlayerList.razor                         # List players
    │   │   ├── PlayerDetail.razor                       # Player details
    │   │   └── PlayerForm.razor                         # Create/edit player
    │   │
    │   ├── Badges/
    │   │   ├── BadgeList.razor                          # List badges
    │   │   ├── BadgeDetail.razor                        # Badge details
    │   │   └── BadgeForm.razor                          # Create/edit badge
    │   │
    │   ├── Rules/
    │   │   ├── RuleList.razor                           # List rules
    │   │   ├── RuleDetail.razor                         # Rule details
    │   │   └── RuleForm.razor                           # Create/edit rule
    │   │
    │   ├── Events/
    │   │   ├── EventList.razor                          # List events
    │   │   └── EventForm.razor                          # Create event
    │   │
    │   ├── Leaderboard.razor                            # Leaderboard page
    │   └── Home.razor                                   # Home page
    │
    ├── Services/                                        # HttpClient API wrappers
    │   ├── ApplicationApiClient.cs                      # Calls Application API
    │   ├── PlayerApiClient.cs                           # Calls Player API
    │   ├── BadgeApiClient.cs                            # Calls Badge API
    │   ├── RuleApiClient.cs                             # Calls Rule API
    │   ├── EventApiClient.cs                            # Calls Event API
    │   └── LeaderboardApiClient.cs                      # Calls Leaderboard API
    │
    ├── _Imports.razor                                   # Global Razor imports
    ├── App.razor                                        # Blazor app root
    └── Program.cs                                       # Blazor WebAssembly bootstrap
```