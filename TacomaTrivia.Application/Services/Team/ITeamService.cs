using TacomaTrivia.Application.Models.Team;
using TacomaTrivia.Domain.AggregateRoots.Teams;

namespace TacomaTrivia.Application.Services.Teams;

public interface ITeamService
{
    public Task<TeamDto?> GetByIdAsync(Guid teamId, CancellationToken ct);
    public Task<IReadOnlyList<TeamDto>> GetTeamsForUserAsync(Guid userId, CancellationToken ct);
    public Task<Guid> CreateTeamAsync(string name, Guid ownerUserId, CancellationToken ct);
    public Task AddMemberAsync(Guid teamId, Guid userId, TeamRole role, CancellationToken ct);
    public Task RemoveMemberAsync(Guid teamId, Guid userId, CancellationToken ct);
    public Task ChangeMemberRoleAsync(Guid teamId, Guid userId, TeamRole newRole, CancellationToken ct);
    public Task DeleteTeamAsync(Guid teamId, CancellationToken ct);

    
    public Task<IReadOnlyList<TeamDto>> SearchTeamAsync(string query, int page, int size, CancellationToken ct);
    
}   