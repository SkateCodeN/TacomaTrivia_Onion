using System.ComponentModel.DataAnnotations;

// This is an incoming payload from the client, when 
// a TeamRecord is created
namespace TacomaTrivia.Application.Models.Team;
public sealed class CreatedTeamRecordDTO
{
    public required string TeamName {get; init;}

    public required DateOnly RecordDate {get; init;}

    public int? Points {get; init;}
    
    public int? Placed {get; init;}

    public Guid TeamId{ get; init;}
    public Guid VenueId{get; init;} 

}
    
