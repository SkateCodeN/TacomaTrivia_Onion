using TacomaTrivia.Application.Models.Team;
using TacomaTrivia.Domain.AggregateRoots.Teams;

namespace TacomaTrivia.Application.Contracts.Team;

// Here we list the use cases for our Team.
public interface ITeamRepository
{
    //Queries
    Task<TriviaTeam?> GetTeamByIdAsync(Guid teamId, CancellationToken ct);
    Task<IReadOnlyList<TeamDto>> GetTeamsForUserAsync(Guid userId, CancellationToken ct);

    // Should a user not be allowed to search for a team?
    // Task<IReadOnlyList<TeamDTO>> SearchTeamAsync(string? q, int page, int pageSize, CancellationToken ct);

    // Only verified users can add a team
    // Task<Guid> AddTeamAsync(Team team, CancellationToken ct);

    // Only team owner can update team
    // Task UpdateTeamAsync(Team team, CancellationToken ct);

    // site owner and team owner can delete team
    // Task<bool> DeleteTeamAsync(Guid id, CancellationToken ct);

    //commands
    //Task<Guid> CreateTeamAsync(string name, Guid ownerUserId, CancellationToken ct);

    //Task AddMemberAsync(Guid teamId, Guid userId, TeamRole role, CancellationToken ct);

    // To add a Team to the DB
    Task AddAsync(TriviaTeam team, CancellationToken ct);

    // A team can be updated
    Task UpdateAsync(TriviaTeam team, CancellationToken ct);
    //Task RemoveMemberAsync(Guid teamId, Guid userId, CancellationToken ct);

    //Task ChangeMemberRole(Guid teamId, Guid userId, TeamRole newRole, CancellationToken ct);

    Task DeleteTeamAsync(Guid teamId, CancellationToken ct);

    Task<IReadOnlyList<TriviaTeam>> SearchTeamsAsync(string? q, int page, int size, CancellationToken ct);
}

