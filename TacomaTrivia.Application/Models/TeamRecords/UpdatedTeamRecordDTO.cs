
namespace TacomaTrivia.Application.Models.TeamRecords;

public sealed class UpdatedTeamRecordDTO
{
    public required string TeamName {get; init;}

    public required DateOnly RecordDate {get; init;}

    public int? Points {get; init;}
    
    public int? Placed {get; init;}

    public Guid TeamId{ get; init;}
    public Guid VenueId{get; init;}
}