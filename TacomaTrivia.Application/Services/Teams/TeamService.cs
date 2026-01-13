
using System.Threading.Tasks;
using TacomaTrivia.Application.Contracts.Team;
using TacomaTrivia.Application.Models.Team;
using TacomaTrivia.Domain.AggregateRoots.Teams;
using TacomaTrivia.Application.Abstractions;
namespace TacomaTrivia.Application.Services.Teams;

public sealed class TeamService: ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly ICurrentUser _currentUser;

    // private Team _team;

    public TeamService(ITeamRepository teamRepository, ICurrentUser currentUser)
    {
        _currentUser = currentUser;
        _teamRepository = teamRepository;
    }

    // Query implementation

    //  Get a specific team by id
    public async Task<TeamDto?> GetByIdAsync(Guid teamId, CancellationToken ct)
    {
        if(teamId == Guid.Empty) throw new ArgumentException("Id cannot be Null");
        
        var team = await _teamRepository.GetTeamByIdAsync(teamId, ct);
        if(team is null) throw new InvalidOperationException("Team not found");
        return new TeamDto(
            team.Id,
            team.Name,
            team.DateCreated,
            [.. team.Members.Select(member => new TeamMemberDto(member.UserId, member.Role))]
        );
    }

    // Get teams associated with a certain user
    public async Task<IReadOnlyList<TeamDto>> GetTeamsForUserAsync(Guid userId, CancellationToken ct)
    {
        var teams = await _teamRepository.GetTeamsForUserAsync(userId, ct);

        return teams.Select( team =>
            new TeamDto(
                team.Id,
                team.Name,
                team.DateCreated,
                team.Members
                    .Select( member => new TeamMemberDto(member.UserId, member.Role))
                    .ToList()
            )
        ).ToList();
    }

    public async Task<Guid> CreateTeamAsync(string name, Guid ownerUserId, CancellationToken ct)
    {
        //Application-level rule: only authenticated users can create teams
        if(!_currentUser.IsAuthenticated) throw new UnauthorizedAccessException();

        // Domain creates the aggregate and enforces invariants
        var team = TriviaTeam.Create(
            name: name,
            ownerUserId: ownerUserId
        );

        await _teamRepository.AddAsync(team, ct);

        return team.Id;
    }

    // We are able to add members to our aggregate
    public async Task AddMemberAsync(Guid teamId, Guid userId, TeamRole role, CancellationToken ct)
    {
        var team = await LoadTeamOrThrow(teamId, ct);

        // Application-level authorization
        // if(!team.CanManageMembers(_currentUser.UserId))
        //     throw new UnauthorizedAccessException();

        // Domain enforces:
        // - max team size
        // - duplicate members
        // - role validity

        team.AddMember(userId,_currentUser.UserId, role);

        await _teamRepository.UpdateAsync(team, ct);

    }
    
    public async Task RemoveMemberAsync(Guid teamId, Guid userId, CancellationToken ct)
    {
        var team = await LoadTeamOrThrow(teamId, ct);

        //redundant, domain already checks this
        // if(!team.CanManageMembers(_currentUser.UserId))
        //     throw new UnauthorizedAccessException();

        //Domain enforces:
        // - cannot remove last admin
        // - cannot remover owner
        team.RemoveMember(_currentUser.UserId,userId);

        await _teamRepository.UpdateAsync(team,ct);
    }

    public async Task ChangeMemberRoleAsync(Guid teamId, Guid userId, TeamRole newRole, CancellationToken ct)
    {
        var team = await LoadTeamOrThrow(teamId, ct);

        // We change our aggregate at the domain level
        team.ChangeMemberRole(userId,_currentUser.UserId, newRole);

        // the repo is the middleman for the DB, so now 
        // we have to update the DB
        await _teamRepository.UpdateAsync(team, ct);
    }
    private async Task<TriviaTeam> LoadTeamOrThrow(Guid teamId, CancellationToken ct)
    {
        var team = await _teamRepository.GetTeamByIdAsync(teamId, ct);
        if(team is null)
            throw new InvalidOperationException("Team Not found");

        return team;
    }
   
   public async Task DeleteTeamAsync(Guid teamId, CancellationToken ct)
    {
        var team = await LoadTeamOrThrow(teamId, ct);

        var teamOwnerId = team.GetTeamOwner();
        // Only owner can delete team
        if(_currentUser.UserId != teamOwnerId)
            throw new UnauthorizedAccessException();

        await _teamRepository.DeleteTeamAsync(team.Id, ct);

    }

    public async Task<IReadOnlyList<TeamDto>> SearchTeamAsync(string? query, int page, int size, CancellationToken ct)
    {
        var teams = await _teamRepository.SearchTeamsAsync(query,page, size, ct);
        return teams.Select(
            v => new TeamDto(
                v.Id,
                v.Name,
                v.DateCreated,
                v.Members
                 .Select( m => new TeamMemberDto(
                        m.UserId,
                        m.Role
                 ))
                 .ToList()
            )
        )
        .ToList();
    }
}


