namespace TacomaTrivia.Application.Models.Team;
public sealed class RemoveMemberReqDto
{
    public required Guid UserId {get; init;}
    public required Guid TeamId {get; init;}
}