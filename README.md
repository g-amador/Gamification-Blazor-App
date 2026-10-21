# Gamification Blazor APP
Gamification Blazor APP to integrate in other projects. Allows to manage players and give points and badges to players when some events raises.

## 📑 <a name="toc">Table of Contents</a>

1. [📂 Project Architecture](#project-architecture)
2. [🤝 Contributing](#contributing)
3. [📄 License](#license)
4. [📬 Contact](#contact)

---

## 📂 <a name="project-architecture">Project Architecture</a>

```text
Gamification-Blazor-App/
├── AI.md                           # Source of truth: conventions, rules, current state & roadmap
├── ARCHITECTURE.md                 # Detailed project architecture & directory structure
├── Gamification API.postman_collection.json
├── LICENSE                         # License details
├── README.md                       # Main project documentation
└── src/
    ├── GamificationSolution.slnx    # .slnx (XML) solution file — all 6 projects

    ├── Gamification.Domain         # Entities + enums (no EF, no DTOs)
    │   ├── Entities
    │
    ├── Gamification.Application    # DTOs, service + repository interfaces, services
    │   ├── DTOs
    │   ├── Interfaces
    │   └── Services
    │
    ├── Gamification.Infrastructure # DbContext (InMemory today, LocalDB/TSQL later), repositories
    │   ├── Data
    │   ├── DependencyInjection
    │   └── Repositories
    │
    ├── Gamification.API            # REST controllers + OpenAPI/Scalar
    │   └── Controllers
    │
    ├── Gamification.BlazorUI       # Blazor WASM UI
    │   ├── Layout
    │   └── Pages
    │
    └── Gamification.Application.Tests  # xUnit + Moq service tests
```

For a complete, file-by-file breakdown of the repository, see [ARCHITECTURE.md](https://github.com/g-amador/Gamification-Blazor-App/blob/main/ARCHITECTURE.md).

[Back to Table of Contents](#toc)

---

## 🤝 <a name="contributing">Contributing</a>

Contributions are welcome, including:

- Bug reports
- Feature requests
- Documentation improvements

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

---