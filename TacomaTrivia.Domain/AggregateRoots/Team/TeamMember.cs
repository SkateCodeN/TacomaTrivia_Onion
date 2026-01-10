namespace TacomaTrivia.Domain.AggregateRoots.Teams;

// Entity inside the team aggregate, has no meaning 
// outside of the team aggregate root.

public sealed class TeamMember
{
    public Guid UserId{get; private set;}

    public Guid TeamId{get; private set;}

    public TeamRole Role {get; private set;}

    // Internal constructor? 
    internal TeamMember(Guid userId, TeamRole role, Guid teamId)
    {
        UserId = userId;
        Role = role;
        TeamId = teamId;
    }

    internal void ChangeRole (TeamRole role)
    {
        Role = role;
    }
}