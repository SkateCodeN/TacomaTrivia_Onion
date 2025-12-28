using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using TacomaTrivia.Application.Contracts.Teams;
using TacomaTrivia.Domain.AggregateRoots;
using TacomaTrivia.Infrastructure.Context;
namespace TacomaTrivia.Infrastructure.Repositories.Postgres;

// Dependence is the EF core library for Postgres

public sealed class EFTeamRecordRepo(TeamRecordDBContext db) : ITeamRecordRepository
{
    private readonly TeamRecordDBContext _db = db;

    public Task<TeamRecord?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.TeamRecords.AsNoTracking().FirstOrDefaultAsync(record => record.Id == id, ct);

    public async Task<IReadOnlyList<TeamRecord>> SearchAsync(string? q, int page, int size, CancellationToken ct )
    {
        var query = _db.TeamRecords.AsNoTracking();
        if(!string.IsNullOrWhiteSpace(q))
            query = query.Where( v => EF.Functions.ILike(v.TeamName, $"%{q}%"));

            return await query.OrderBy(v => v.TeamName)
                                .Skip((page - 1) * size )
                                .Take(size)
                                .ToListAsync(ct);
    }

    public async Task<Guid> AddAsync(TeamRecord record, CancellationToken ct)
    {
        _db.TeamRecords.Add(record);
        await _db.SaveChangesAsync(ct);
        return record.Id;
    }

    public async Task UpdateAsync(TeamRecord record, CancellationToken ct)
    {
        var entry = _db.Entry(record);
        if(entry.State == EntityState.Detached)
        {
            _db.Attach(record);
            entry.State = EntityState.Modified;
        }
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var affected = await _db.TeamRecords
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync(ct);
        return affected > 0;
    }
}

