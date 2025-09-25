# TacomaTrivia — Architecture Diagrams (Mermaid)

## Layers (Clean Architecture)
```mermaid
flowchart LR
  subgraph Domain
    D1[Entities]
    D2[Value Objects]
    D3[Interfaces]
    D4[Domain Services]
    D5[Events]
  end
  subgraph Application
    A1[Use Cases / Services]
    A2[DTOs / Mappers]
    A3[Validators]
    A4[Repo Ports]
  end
  subgraph Infrastructure
    I1[EF Core DbContext]
    I2[EF Repositories]
    I3[Fluent Config]
    I4[Integrations]
  end
  subgraph WebAPI
    W1[Controllers]
    W2[Req/Resp Models]
    W3[Middleware]
    W4[DI Root]
  end
  Domain --> Application
  Application --> Infrastructure
  Application --> WebAPI
  Infrastructure --> IaaS[(MySQL in Docker)]
  WebAPI --> Clients[(React Frontend)]


classDiagram
  class Venue {
    +Guid Id
    +string Name
    +string Address
    +string City
    +string State
    +string PostalCode
    +DayOfWeek TriviaDay
    +TimeOnly TriviaStartTime
    +bool IsOpen
    +void UpdateHours(Hours hours)
    +void ScheduleTrivia(DayOfWeek day, TimeOnly start)
  }
  class MenuItem {
    +Guid Id
    +string Title
    +decimal Price
    +string Description
  }
  class Hours {
    +TimeOnly Open
    +TimeOnly Close
    +bool Is24Hours
  }
  Venue "1" o-- "*" MenuItem : aggregates
