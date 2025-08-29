# Gamification Blazor APP
Gamification Blazor APP to integrate in other projects. Allows to manage players and give points and badges to players when some events raises.

## Project structure

Gamification-App-Blazor/                    # Solution folder
│
├─ GamificationAppBlazor.Client/            # Blazor client-side Pages/Services
│  ├─ Properties/
│  │   └─ launchSettings.json
│  ├─ wwwroot/
│  │   ├─ css/
│  │   │   └─ app.css
│  │   ├─ icon-192.png
│  │   └─ index.html
│  ├─ Layout/
│  │   └─ MainLayout.razor
│  ├─ Pages/
│  │   ├─ Applications/
│  │   │   ├─ ApplicationList.razor
│  │   │   ├─ ApplicationDetail.razor
│  │   │   └─ ApplicationForm.razor
│  │   ├─ Players/
│  │   │   ├─ PlayerList.razor
│  │   │   ├─ PlayerDetail.razor
│  │   │   └─ PlayerForm.razor
│  │   ├─ Badges/
│  │   │   ├─ BadgeList.razor
│  │   │   ├─ BadgeDetail.razor
│  │   │   └─ BadgeForm.razor
│  │   ├─ Rules/
│  │   │   ├─ RuleList.razor
│  │   │   ├─ RuleDetail.razor
│  │   │   └─ RuleForm.razor
│  │   ├─ Events/
│  │   │   ├─ EventList.razor
│  │   │   └─ EventForm.razor
│  │   ├─ Leaderboard.razor
│  │   └─ Home.razor
│  ├─ Services/
│  │   ├─ IApplicationService.cs
│  │   ├─ ApplicationService.cs
│  │   ├─ IPlayerService.cs
│  │   ├─ PlayerService.cs
│  │   └─ ...
│  ├─ _Imports.razor
│  ├─ App.razor
│  └─ Program.cs                        # Blazor WebAssembly UI initialization
│
├─ GamificationAppBlazor.Server/        # API logic
│  ├─ Controllers/
│  │   ├─ ApplicationsController.cs
│  │   ├─ PlayersController.cs
│  │   ├─ BadgesController.cs
│  │   ├─ RulesController.cs
│  │   ├─ EventsController.cs
│  │   └─ LeaderboardController.cs
│  ├─ Data/
│  │   ├─ GamificationDbContext.cs
│  │   └─ SeedData.cs
│  ├─ Models/
│  │   ├─ Application.cs
│  │   ├─ Player.cs
│  │   ├─ Badge.cs
│  │   ├─ Rule.cs
│  │   └─ Event.cs
│  ├─ Services/
│  │   ├─ ApplicationService.cs
│  │   ├─ PlayerService.cs
│  │   ├─ BadgeService.cs
│  │   ├─ RuleService.cs
│  │   └─ EventService.cs
│  ├─ Mappings/
│  │   └─ AutoMapperProfiles.cs
│  └─ Program.cs                        # API initialization, Swagger, database, server-side DI
│
├─ GamificationAppBlazor.Shared/        # Shared Models/DTOs
│  ├─ DTOs/
│  │   ├─ ApplicationDto.cs
│  │   ├─ PlayerDto.cs
│  │   ├─ BadgeDto.cs
│  │   ├─ RuleDto.cs
│  │   ├─ EventDto.cs
│  │   └─ LeaderboardDto.cs
│  └─ Enums/
│      └─ EventType.cs
│
└─ GamificationAppBlazor.sln            # Solution file