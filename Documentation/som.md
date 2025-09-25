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
