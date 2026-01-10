using System.ComponentModel.DataAnnotations;

// This is an incoming payload from the client, when 
// a TeamRecord is created
namespace TacomaTrivia.Application.Models.Team;
public sealed class CreatedTeamDto
{
    public required string TeamName {get; init;}
    public required Guid TeamOwnerId {get; init;}

    
}
    
