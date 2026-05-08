# Tacoma Trivia Backend

## Overview

Production-ready .NET 8.0 Onion Architecture trivia platform with Auth0 authentication and venue management.

## Architecture Pattern

Follows **Hexagonal (Onion) Architecture** to ensure:
- Separation of concerns
- Testability  
- Clean separation between domain logic and external dependencies

## Tech Stack

- **Backend**: .NET 8.0 (C#) with ASP.NET Core API
- **Authentication**: Auth0 for secure user management
- **Frontend**: React.js client application (separate repo)

## Repository Structure

```
TacomaTrivia.Domain         - Core business entities and invariants
TacomaTrivia.Application    - Use case layer with service contracts
TacomaTrivia.Infrastructure  - Data persistence and repositories
TacomaTrivia.Api            - ASP.NET Core controllers
IntegrationTests            - API integration tests
UnitTests                   - Application unit tests
```

## Quick Start

### Prerequisites
- .NET 8.0 SDK installed
- Git for cloning repository
- Visual Studio 2022+ or Rider

### Setup Steps
1. Clone the repository: `git clone https://github.com/SkateCodeN/TacomaTrivia_Onion`
2. Open solution file: `TacomaTrivia.sln` in Visual Studio
3. Update database connection string in `AppDbContext.cs`
4. Build and run: `dotnet build && dotnet run`

### Container Option
Set Docker=true in project files to use container deployment mode.

## API Endpoints

- Auth0 login at `/api/auth0`
- Trivia venues CRUD at `/api/venues`
- Products management at `/api/products`

## License

TBD - Contact owner for commercial use information.

**Last Updated**: May 8, 2025