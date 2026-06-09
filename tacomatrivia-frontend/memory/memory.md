# TacomaTrivia Frontend — Project Memory

## Stack & Technologies
| Category | Tool |
|---|---|
| Framework | React 19 |
| Build Tool | Vite 7 |
| UI Library | Mantine Core 8 (`@mantine/core`, `@mantine/hooks`, `@mantine/notifications`, `@mantine/form`) |
| Icons | Tabler Icons React (`@tabler/icons-react`) |
| Routing | React Router DOM 7 |
| HTTP Client | Axios |
| Linting | ESLint 9 |
| CSS | CSS Modules (`.module.css`) + global `index.css` |
| Default Color Scheme | Dark |

## Vite Config
- **Alias resolution:**
  - `@app` → `src/app`
  - `@features` → `src/features`
  - `@shared` → `src/shared`
  - `@widgets` → `src/widgets`
  - `@assets` → `src/assets`
- **Dev proxy:** `/api` → `http://localhost:5067` (backend)

## Directory Structure

```
tacomatrivia-frontend/
├── public/
├── src/
│   ├── main.jsx              ← Entry point (StrictMode, MantineProvider)
│   ├── theme.js              ← Mantine custom theme (brand colors, Lexend font)
│   ├── index.css             ← Global styles
│   ├── app/
│   │   ├── App.jsx           ← Root component (Providers + RouterProvider)
│   │   ├── routes.jsx        ← createBrowserRouter config
│   │   └── Test.jsx
│   ├── components/           ← Shared UI components
│   │   ├── AppNavBar.jsx     ← Navbar with Burger/Drawer (mobile responsive)
│   │   ├── EventCard.jsx
│   │   ├── FeaturedEvents.jsx
│   │   ├── HelpCTA.jsx
│   │   ├── Hero.jsx
│   │   ├── NavAuthComp.jsx
│   │   └── NewsletterCTA.jsx
│   ├── features/             ← Feature-based folders (api, components, pages, helpers)
│   │   ├── Form-Auth/
│   │   │   ├── AuthUser.jsx
│   │   │   ├── LoginForm.jsx
│   │   │   └── LoginPage.jsx
│   │   ├── Teams/
│   │   │   └── components/
│   │   │       └── UserSelectWithCreate.jsx
│   │   ├── UI/
│   │   │   ├── components/
│   │   │   │   ├── BecomeModCTA.jsx
│   │   │   │   ├── Hero.jsx
│   │   │   │   └── HeroTacoma.module.css
│   │   │   └── pages/
│   │   │       └── Home.jsx
│   │   └── venues/
│   │       ├── api/
│   │       │   └── venuesApi.js
│   │       ├── components/
│   │       │   └── VenuesTable.jsx
│   │       ├── helper functions/
│   │       │   ├── CompareDateTime.js
│   │       │   ├── ConvertToDateTime.js
│   │       │   ├── DayConverter.js
│   │       │   └── TimeConverter.js
│   │       └── pages/
│   │           └── VenuesListPage.jsx
│   ├── shared/               ← Cross-cutting utilities
│   │   ├── api/
│   │   │   └── http.js       ← Axios instance / base config
│   │   ├── hooks/
│   │   │   └── useDebounce.js
│   │   └── ui/
│   │       └── RowsPerPageSelect.jsx
│   ├── widgets/              ← Layout shells
│   │   └── Layout.jsx        ← Shell with AppNavbar + Outlet
│   ├── providers/
│   │   └── Providers.jsx     ← MantineProvider + Notifications wrapper
│   ├── styles/               ← CSS modules for components
│   │   ├── AppNavBar.module.css
│   │   ├── EventCard.module.css
│   │   ├── HelpCTA.module.css
│   │   └── Hero.module.css
│   └── assets/               ← Static images
│       ├── MtRainer.jpg
│       ├── MtRainer_William Jacobs.jpg
│       └── shkrabaanthony-mod.jpg
├── index.html
├── vite.config.js
├── eslint.config.js
├── jsconfig.json
└── package.json
```

## Routing
```
/                  → Layout → Home
/venues            → Layout → VenuesListPage (lazy)
/login             → Layout → LoginPage
/auth/user         → AuthUser
*                  → NotFound
```

## Component Patterns
- **Functional components only** (no class components)
- **Mantine components** for UI (Container, Group, Burger, Drawer, Stack, Button, Text, Box, Collapse, etc.)
- **CSS Modules** for component-specific styles (imported as `styles from './X.module.css'`)
- **Inline styles** used sparingly for quick overrides
- **`@mantine/hooks`** for hooks (useDisclosure, etc.)
- **`@mantine/form`** for form validation
- **Lazy loading** for route components (`React.lazy`)
- **Suspense** fallback on the router

## Theme (`theme.js`)
- **Font:** Lexend, sans-serif
- **Headings:** fontWeight 700
- **Brand palette (10 shades):** warm gradient (#fdf3e6 → #4a065d)
- **Badge colors:** red, yellow, green
- **Primary color:** `brand`

## API Layer
- `shared/api/http.js` — Axios base instance
- `features/venues/api/venuesApi.js` — Venues-specific API calls
- Dev proxy: `/api/*` forwarded to backend at `localhost:5067`

## Helpers
- `CompareDateTime.js` — DateTime comparison utility
- `ConvertToDateTime.js` — DateTime conversion utility
- `DayConverter.js` — Day name conversion
- `TimeConverter.js` — Time format conversion
- `useDebounce.js` — Custom debounce hook
