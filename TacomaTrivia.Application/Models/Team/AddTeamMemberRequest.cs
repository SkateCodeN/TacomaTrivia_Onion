namespace TacomaTrivia.Application.Models.Team;
public sealed class AddTeamMemberRequest
{
    public required TeamRoleDto Role {get; init;}
    public required Guid UserId {get; init;}
    public required Guid TeamId {get; init;}
}