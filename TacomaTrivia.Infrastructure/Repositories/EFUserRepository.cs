using Microsoft.EntityFrameworkCore;
using TacomaTrivia.Application.Contracts;
using TacomaTrivia.Application.Models;
using TacomaTrivia.Domain;

namespace TacomaTrivia.Infrastructure.Repositories;
/// <summary>EF Core repository implementation (MS SQL Server oriented).</summary>
public sealed class EFUserRepository(UserDbContext db) : IUserRepository
{
    private readonly UserDbContext _db = db;

    public Task<TTUser?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.TTUsers.AsNoTracking().FirstOrDefaultAsync(usr => usr.Id == id, ct);


    public async Task<IReadOnlyList<TTUser>> SearchAsync(string? q, int page, int size, CancellationToken ct)
    {
        var qry = _db.TTUsers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
        {
            // Changed ILike is Postgress only, Like works on all providers
            // WHY: SQL Server doesn’t support ILIKE. Most SQL Server 
            // collations are already CI (case-insensitive).
            // We add an explicit COLLATE to be deterministic for 
            // case/diacritic behavior even if DB default differs.

            // case-insensitive, accent-insensitive, supplementary-chars aware
            const string CI_AI = "Latin1_General_100_CI_AI_SC";
            qry = qry.Where(
                usr => EF.Functions.Like(
                    // CHANGED: force per-expression collation for portable behavior
                    EF.Functions.Collate(usr.Name, CI_AI), $"%{q}%"
                )
            );
        }

        // same semantics; SQL Server translates to ORDER BY ... OFFSET/FETCH
        return await qry.OrderBy(usr => usr.Name)
                        .Skip((page - 1) * size)
                        .Take(size)
                        .ToListAsync(ct);
    }

    public async Task<Guid> AddAsync(TTUser user, CancellationToken ct)
    {
        _db.TTUsers.Add(user);
        await _db.SaveChangesAsync(ct);
        return user.Id;
    }

    public async Task UpdateAsync(TTUser user, CancellationToken ct)
    {
        var entry = _db.Entry(user);
        if (entry.State == EntityState.Detached)
        {
            _db.Attach(user);
            entry.State = EntityState.Modified;
        }
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        // stays the same for Postgres and MS SQL
        // ExecuteDeleteAsync is supported EF Core v7 and v8
        var affected = await _db.TTUsers
                            .Where(usr => usr.Id == id)
                            .ExecuteDeleteAsync(ct);
        return affected > 0;
    }
}

