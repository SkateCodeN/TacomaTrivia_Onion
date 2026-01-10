using System.Runtime.CompilerServices;

namespace TacomaTrivia.Domain.AggregateRoots.Teams;

//Aggregate root representing a trivia team,
// each team owns its own member list, roles and team invariants

public sealed class TriviaTeam
{
    private readonly List<TeamMember> _members = new();

    public Guid Id {get; private set;} = Guid.NewGuid();
    public string Name {get; private set;} = default!;

    public Guid TeamOwnerId { get; private set;}
    public DateOnly DateCreated {get; private set;}

    public IReadOnlyCollection<TeamMember> Members => _members.AsReadOnly();

    private const int MinMembers = 2;
    private const int MaxMembers = 6;

    //Factory menthod ensures a valid initial state
    // A team must start with an owner

    public static TriviaTeam Create(string name, Guid ownerUserId)
    {
        if(string.IsNullOrWhiteSpace(name))
        throw new ArgumentException("Team Name required");

        var team = new TriviaTeam
        {
            Name = name.Trim(),
            DateCreated = DateOnly.FromDateTime(DateTime.Now),
            TeamOwnerId = ownerUserId
        };

        team._members.Add(new TeamMember(ownerUserId, TeamRole.Owner,team.Id)); 
        // also make sure the team has its owners id
        //team.TeamOwnerId = ownerUserId;

        return team;
    }

    //Domain rule: only owner/admin can manage members
    public bool CanManageMembers(Guid userId)
    {
        return _members.Any(member =>
        member.UserId == userId &&
        (member.Role == TeamRole.Owner || member.Role == TeamRole.Admin));
    }

    //Get the ownerID, so we know who owns the team
    public Guid GetTeamOwner()
    {
        var owner  = _members.Single(member => member.Role == TeamRole.Owner);
        return owner.UserId;
    }


    //Bool to see if 

    // Domain rule: Only Team Admin can add members
    public void AddMember(Guid requestingUserId, Guid newUserId, TeamRole role)
    {
        EnsureCanManageMembers(requestingUserId);
        if(_members.Count >= MaxMembers)
            throw new InvalidOperationException("Team member limit reached");

        if(_members.Any(member => member.UserId == newUserId))
            throw new InvalidOperationException("User aready in team");

        _members.Add(new TeamMember(newUserId, role, Id));

    }

    // Domain rule: Only Team Admin can add members
    public void RemoveMember(Guid requestingUserId, Guid userId)
    {
        EnsureCanManageMembers(requestingUserId);

        var member = _members.SingleOrDefault(member => member.UserId == userId)
        ?? throw new InvalidOperationException("Member not found");

        if(member.Role == TeamRole.Owner && 
            _members.Count(member => member.Role == TeamRole.Owner) == 1)
                throw new InvalidOperationException("Team nust have an owner");
        
    
        _members.Remove(member);

        if(_members.Count < MinMembers)
            throw new InvalidOperationException("Team must have at least two members");
    }

    // Domain rule: Only Team Admins can change member roles
    public void ChangeMemberRole(Guid requestingUserId, Guid userId, TeamRole role)
    {
        EnsureCanManageMembers(requestingUserId);

        var member = _members.Single(member => member.UserId == userId);
        member.ChangeRole(role);
    }
    
    // Helper function to save extra calls, we make sure the user is part of the owner or admin
    private void EnsureCanManageMembers(Guid userId)
    {
        if(!CanManageMembers(userId))
            throw new UnauthorizedAccessException("User cannot manage team member");
    }
}