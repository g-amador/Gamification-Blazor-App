# Gamification Blazor APP

Add gamification to your applications in a simple way: register applications, manage players, and give points and badges to players when some events are raised. The API is consumer-agnostic — Web, Mobile, OutSystems and future integrations can consume it with a single contract.

## 📑 <a name="toc">Table of Contents</a>

1. [⚙️ How it works](#how-it-works)
2. [🔌 API overview](#api-overview)
3. [🚀 Getting started](#getting-started)
4. [📂 Project Architecture](#project-architecture)
5. [🤝 Contributing](#contributing)
6. [📄 License](#license)
7. [📬 Contact](#contact)

---

## ⚙️ <a name="how-it-works">How it works</a>

1. **Register an application** — the application is the entry point of the API. It has a unique `apiKey` and `apiPassword`, used for authentication on every request via HTTP headers.
2. **Create players, badges, event types, badge categories and rules** — all resources belong to an application.
3. **Post events** — when something happens to a player, post an event with its type. The gamification engine browses the rules and awards the corresponding points and badges automatically (a badge already owned is not awarded again).
4. **Read the leaderboard** — the top 5 players of the application, ordered by points.

## 🔌 <a name="api-overview">API overview</a>

| Resource | Endpoints |
| --- | --- |
| Applications | `GET /applications`, `GET /application`, `POST /application`, `PUT /application`, `DELETE /application` |
| Players | `GET /players`, `GET /players/{id}`, `GET /players/{id}/badges`, `POST /players`, `PUT /players/{id}`, `DELETE /players/{id}` |
| Badges | `GET /badges`, `GET /badges/{id}`, `POST /badges`, `PUT /badges/{id}`, `DELETE /badges/{id}` |
| Badge Categories | `GET /badge-categories`, `GET /badge-categories/{id}`, `POST /badge-categories`, `PUT /badge-categories/{id}` |
| Rules | `GET /rules`, `GET /rules/{id}`, `POST /rules`, `PUT /rules/{id}`, `DELETE /rules/{id}` |
| Events (immutable) | `GET /events`, `GET /events/{id}`, `POST /events` |
| Event Types | `GET /event-types`, `GET /event-types/{id}`, `POST /event-types`, `PUT /event-types/{id}` |
| Leaderboard | `GET /leaderboard` |

Every resource is scoped to the application identified by the credentials in the request headers:

```
apiKey
apiPassword
```

A full, ready-to-run Postman collection covers **every endpoint above**: [`Gamification API.postman_collection.json`](https://github.com/g-amador/Gamification-Blazor-App/blob/main/Gamification%20API.postman_collection.json).

In Development the API also serves an OpenAPI description and an interactive Scalar UI at:

```
/scalar
/openapi/v1.json
```

## 🚀 <a name="getting-started">Getting started</a>

Prerequisites: [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

```bash
# restore + build everything
dotnet build src/GamificationSolution.slnx

# run the API (uses SQL Server LocalDB by default; set Database:UseInMemory=true in appsettings.json for EF InMemory)
dotnet run --project src/Gamification.API
# ➜ http://localhost:5264  (Scalar UI: /scalar in Development)

# run the unit tests
dotnet test src/GamificationSolution.slnx

# run the Blazor WASM UI shell
dotnet run --project src/Gamification.BlazorUI
```

The API runs on **SQL Server LocalDB** (`(localdb)\MSSQLLocalDB`) by default — EF Core migrations are applied automatically at startup (`20261025101441_InitialCreate`). Set `Database:UseInMemory` to `true` in `appsettings.json` to run on the EF Core **InMemory** database instead, and use the `DefaultConnection` string to point at any full TSQL SQL Server instance. The repo-local [`nuget.config`](nuget.config) restricts package sources to nuget.org so builds never depend on machine-level private feeds. See [AI.md → DATABASE STRATEGY](https://github.com/g-amador/Gamification-Blazor-App/blob/main/AI.md).

## 📂 <a name="project-architecture">Project Architecture</a>

```text
Gamification-Blazor-App/
├── AI.md                             # Machine/agent briefing: conventions, current state, roadmap
├── ARCHITECTURE.md                   # Detailed, file-level architecture of the repository
├── Directory.Packages.props          # Central package management — single source for NuGet versions
├── Gamification API.postman_collection.json   # Postman collection: one call per REST method
├── LICENSE                           # License details
├── nuget.config                      # Repo-local package sources (nuget.org only)
├── README.md                         # This file
└── src/
    ├── GamificationSolution.slnx     # .slnx (XML) solution — all 6 projects
    ├── Gamification.Domain           # Entities + enums (no EF, no DTOs)
    ├── Gamification.Application      # DTOs, service & repository interfaces, services, tests targets
    ├── Gamification.Infrastructure   # DbContext (+ EF Migrations; LocalDB default / InMemory optional), repositories
    ├── Gamification.API              # REST controllers, OpenAPI + Scalar
    ├── Gamification.BlazorUI         # Blazor WASM UI
    └── Gamification.Application.Tests # xUnit + Moq service tests
```

For the file-by-file breakdown of the repository, see [ARCHITECTURE.md](#); for development conventions and the agent workflow, see [AI.md](#).

[Back to Table of Contents](#toc)

---

## 🤝 <a name="contributing">Contributing</a>

Contributions are welcome, including:

- Bug reports
- Feature requests
- Documentation improvements
- New gamification features (see [AI.md → NEXT PRIORITIES](#) for the current roadmap)

[Back to Table of Contents](#toc)

---

## 📄 <a name="license">License</a>

Gamification-Blazor-App is released under the **MIT License**.

See the [LICENSE](https://github.com/g-amador/Gamification-Blazor-App/blob/main/LICENSE) file for details.

[Back to Table of Contents](#toc)

---

## 📬 <a name="contact">Contact</a>

If you have questions, suggestions, or feedback, feel free to get in touch:

📧 **g.n.p.amador@gmail.com**

If you extend Gamification-Blazor-App, I would be delighted to hear about it.

[Back to Table of Contents](#toc)
