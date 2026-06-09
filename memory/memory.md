# TacomaTrivia_Onion — LLM Quick Reference

## Architecture: Onion (Hexagonal)

```
TacomaTrivia.Api          ← Controllers, Program.cs (DI, auth, middleware), Swagger
    ↕
TacomaTrivia.Application  ← Services, Contracts (interfaces), DTOs/Models
    ↕
TacomaTrivia.Domain       ← Domain entities, business invariants
    ↕
TacomaTrivia.Infrastructure ← EF Core (AppDbContext), Repository implementations
```

**Key principle:** Inner layers (Domain → Application) depend **only** on each other. Outer layers depend inward. Interfaces are defined in Application/Contracts, implementations live in Infrastructure.

---

## Backend Technology Stack

| Category | Technology |
|---|---|
| Runtime | .NET 8 (ASP.NET Core) |
| ORM | Entity Framework Core |
| Database | PostgreSQL (via Npgsql) |
| Auth | JWT Bearer + Auth0 OIDC |
| API Docs | Swagger/OpenAPI |
| Testing | xUnit (UnitTests + IntegrationTests) |

---

## Layer Breakdown

### `TacomaTrivia.Domain`
- **Purpose:** Pure domain entities with business invariants
- **Files:**
  - `TacomaVenue.cs` — Domain entity with factory (`Create`) and `Update` methods, private setters, validation invariants
- **Pattern:** Domain-driven design — entities encapsulate their own validation logic in factory/update methods

### `TacomaTrivia.Application`
- **Purpose:** Application services + contracts (interfaces)
- **Folders:**
  - `Contracts/` — Interface definitions (e.g., `IVenueRepository`, `IVenueService`)
  - `Services/` — Service implementations (`VenueService`)
  - `Models/` — DTOs (e.g., `VenueDTO`, `UpdatedVenue`)
- **Pattern:** Service layer orchestrates business logic; interfaces decouple from infrastructure

### `TacomaTrivia.Infrastructure`
- **Purpose:** Persistence and external concerns
- **Files:**
  - `AppDbContext.cs` — EF Core context (Npgsql)
  - `Repositories/` — Concrete repository implementations (e.g., `EfVenueRepository` implements `IVenueRepository`)
- **Pattern:** Repository pattern abstracts data access; DI injects implementations at runtime

### `TacomaTrivia.Api`
- **Purpose:** HTTP entry point
- **Files:**
  - `Program.cs` — DI registration, auth middleware, Swagger, SPA fallback
  - `Controllers/` — REST endpoints
  - `Auth/` — Auth config (`AuthConfig`, `Auth0Constants`)
- **Pattern:** Minimal API setup; static files served from `wwwroot` for deployed frontend; `MapFallbackToFile("index.html")` for client-side routing

---

## Auth Configuration

- **JWT Bearer:** Symmetric key from config (`Jwt:Key`), issuer/audience = `"tacomatrivia.com"`
- **Auth0 OIDC:** Domain, ClientId, ClientSecret from config; role claim type = `"https://tacomatrivia.com/roles"`
- **Policies:**
  - `ReadOnly` — allows `Admin` + `TestUser` roles
  - `AdminOnly` — requires `Admin` role
- **AuthConfig** class defines policy names and role constants

---

## Testing

| Project | Purpose |
|---|---|
| `TacomaTrivia.UnitTests` | Domain invariants + service logic; uses `InMemoryVenueRepository` (fake) |
| `TacomaTrivia.IntegrationTests` | API endpoints + EF Core with real PostgreSQL (via `PostgresFixture`) |

---

## Frontend (`tacomatrivia-frontend`)

| Category | Technology |
|---|---|
| Framework | React 19 (JSX) |
| Build | Vite 7 |
| UI Library | Mantine Core v8 (`@mantine/core`, `@mantine/hooks`, `@mantine/form`, `@mantine/notifications`) |
| Icons | Tabler Icons (`@tabler/icons-react`) |
| Routing | React Router DOM v7 |
| HTTP | Axios |
| Styling | Mantine components + CSS Modules (`.module.css`) |
| Linting | ESLint + `eslint-plugin-react-hooks` |

### Frontend Structure
```
src/
├── app/            — App.jsx, routes.jsx (routing)
├── assets/         — Images
├── components/     — Global reusable components (AppNavBar, EventCard, Hero, etc.)
├── features/       — Feature-based modules
│   ├── Form-Auth/  — AuthUser, LoginForm, LoginPage
│   ├── Teams/      — Team-related components
│   ├── UI/         — Home page, BecomeModCTA
│   └── venues/     — venuesApi.js, VenuesTable, helper functions, VenuesListPage
├── pages/          — Home.jsx
├── providers/      — Providers.jsx (Mantine context providers)
├── shared/         — Cross-cutting utilities
│   ├── api/        — http.js (axios instance)
│   ├── hooks/      — useDebounce.js
│   └── ui/         — RowsPerPageSelect.jsx
├── styles/         — CSS Modules per component
├── widgets/        — Layout.jsx
├── theme.js        — Mantine theme config
├── App.css         — Global styles
├── index.css       — Global styles
└── main.jsx        — Entry point
```

### Frontend Patterns
- **Feature-based organization:** Each domain (venues, auth, teams) has its own `features/` folder with `api`, `components`, `pages`, and `helper functions` subfolders
- **Shared utilities:** Split into `shared/api` (HTTP client), `shared/hooks` (custom hooks), `shared/ui` (reusable UI primitives)
- **API layer:** Axios instance in `shared/api/http.js` + per-feature API files (e.g., `venuesApi.js`)
- **Styling:** Mantine components for layout/forms + CSS Modules for component-specific styles

---

## Key Conventions

1. **C# namespaces** match project names (`TacomaTrivia.Domain`, `TacomaTrivia.Application`, etc.)
2. **Entity IDs** use `Guid` (e.g., `TacomaVenue.Id`)
3. **Time handling:** `TimeOnly` for time-of-day fields, nullable where optional
4. **DI registration:** Scoped services (`AddScoped<IVenueRepository, EfVenueRepository>()`)
5. **Entity Framework:** Npgsql provider, migrations managed via EF Core tools
6. **Config sources:** `appsettings.json` → `appsettings.Development.json` → User Secrets → Environment variables
7. **Connection string key:** `ConnectionStrings__Postgres` (env var) / `"Postgres"` (appsettings)
8. **Frontend entry:** `main.jsx` → `Providers.jsx` (Mantine) → `App.jsx` → `routes.jsx`
9. **C# partial class:** `Program.cs` uses `public partial class Program` for test accessibility
