namespace TacomaTrivia.Application.Models.TeamRecords;
public sealed record TeamRecordDTO
(
    Guid Id,
    Guid VenueId,
    Guid TeamId,
    DateOnly RecordDate,
    string TeamName,
    int? Points,
    int? Placed
);