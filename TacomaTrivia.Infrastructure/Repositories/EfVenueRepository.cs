using Microsoft.EntityFrameworkCore;
using TacomaTrivia.Application.Contracts;
using TacomaTrivia.Domain;

namespace TacomaTrivia.Infrastructure.Repositories;

/// <summary>EF Core repository implementation.</summary>
public sealed class EfVenueRepository(AppDbContext db) : IVenueRepository
{
    private readonly AppDbContext _db = db;

    public Task<TacomaVenue?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.TacomaVenues.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id, ct);

    public async Task<IReadOnlyList<TacomaVenue>> SearchAsync(string? q, int page, int size, CancellationToken ct)
    {
        var qry = _db.TacomaVenues.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q))
            qry = qry.Where(v => EF.Functions.ILike(v.Name, $"%{q}%"));

        return await qry.OrderBy(v => v.Name)
                        .Skip((page - 1) * size)
                        .Take(size)
                        .ToListAsync(ct);
    }

    public async Task<Guid> AddAsync(TacomaVenue venue, CancellationToken ct)
    {
        _db.TacomaVenues.Add(venue);
        await _db.SaveChangesAsync(ct);
        return venue.Id;
    }

    public async Task UpdateAsync(TacomaVenue venue, CancellationToken ct)
    {
        var entry = _db.Entry(venue);
        if (entry.State == EntityState.Detached)
        {
            _db.Attach(venue);
            entry.State = EntityState.Modified;
        }
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var affected = await _db.TacomaVenues
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync(ct); // server-side delete (EF7+)
        return affected > 0;
    }
}
