namespace TacomaTrivia.Application.Models.Team;
public sealed class ChangeMemberRoleReqDto
{
    public required TeamRoleDto NewRole {get; init;}
    public required Guid UserId {get; init;}
    public required Guid TeamId {get; init;}
}