using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TacomaTrivia.Application.Contracts.Team;
using TacomaTrivia.Domain.AggregateRoots.Teams;
using TacomaTrivia.Application.Models.Team;
using TacomaTrivia.Infrastructure.Context;
namespace TacomaTrivia.Infrastructure.Repositories.Postgres;

public sealed class EFTeamRepo(TeamDbContext db) : ITeamRepository
{
    private readonly TeamDbContext _db = db;
    public Task<TriviaTeam?> GetTeamByIdAsync(Guid teamId, CancellationToken ct)
    => _db.TriviaTeams.AsNoTracking().FirstOrDefaultAsync(team => team.Id == teamId, ct);

    public async Task<IReadOnlyList<TeamDto>> GetTeamsForUserAsync(Guid userId, CancellationToken ct)
    {
        return await _db.TriviaTeams
                        .AsNoTracking()
                        .Where(team =>
                            team.TeamOwnerId == userId ||
                            team.Members.Any(m => m.UserId == userId))
                        .Select(team => new TeamDto(
                            team.Id,
                            team.Name,
                            team.DateCreated,
                            team.Members
                                .Select(m => new TeamMemberDto(
                                    m.UserId,
                                    m.Role
                                ))
                                .ToList()
                        ))
                        .ToListAsync(ct);
    }
    
    public async Task AddAsync(TriviaTeam team, CancellationToken ct)
    {
        _db.TriviaTeams.Add(team);
        await _db.SaveChangesAsync(ct);
        // we could return a team ID to let us know that we created it
    }

    public async Task UpdateAsync(TriviaTeam team, CancellationToken ct)
    {
        var entry = _db.Entry(team);
        if(entry.State == EntityState.Detached)
        {
            _db.Attach(team);
            entry.State = EntityState.Modified;
        }
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteTeamAsync(Guid teamId, CancellationToken ct)
    {
       var affected = await _db.TriviaTeams
            .Where(team => team.Id == teamId)
            .ExecuteDeleteAsync(ct);
             
    }

    // adding ability to search for a specific team via its name
    public async Task<IReadOnlyList<TriviaTeam>> SearchTeamsAsync(string? q, int page, int size, CancellationToken ct)
    {
        var query = _db.TriviaTeams.AsNoTracking();
        if(!string.IsNullOrEmpty(q))
            query = query.Where( v => EF.Functions.ILike(v.Name, $"%{q}%"));

            return await query.OrderBy(v => v.Name)
                            .Skip((page -1) * size)
                            .Take(size)
                            .ToListAsync();
    }
}