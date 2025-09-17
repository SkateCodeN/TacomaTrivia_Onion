using System.Collections.Concurrent;
using TacomaTrivia.Application.Contracts;
using TacomaTrivia.Domain;

public sealed class InMemoryVenueRepository : IVenueRepository
{
    private readonly ConcurrentDictionary<Guid, TacomaVenue> _store = new();

    public Task<Guid> AddAsync(TacomaVenue venue, CancellationToken ct)
    {
        if (venue.Id == Guid.Empty)
            throw new InvalidOperationException("Venue.Id must be set by the domain factory.");
        if (!_store.TryAdd(venue.Id, venue))
            throw new InvalidOperationException($"Duplicate id {venue.Id}");
        return Task.FromResult(venue.Id);
    }

    public Task<TacomaVenue?> GetByIdAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_store.TryGetValue(id, out var v) ? v : null);

    public Task<IReadOnlyList<TacomaVenue>> SearchAsync(string? q, int page, int pageSize, CancellationToken ct)
    {
        IEnumerable<TacomaVenue> src = _store.Values;

        if (!string.IsNullOrWhiteSpace(q))
        {
            var s = q.Trim().ToLowerInvariant();
            src = src.Where(v =>
                (v.Name ?? "").ToLowerInvariant().Contains(s) ||
                (v.Phone ?? "").ToLowerInvariant().Contains(s) ||
                (v.Address ?? "").ToLowerInvariant().Contains(s));
        }

        if (page < 1) page = 1;
        if (pageSize <= 0) pageSize = 25;

        var list = src.OrderBy(v => v.Name)
                      .Skip((page - 1) * pageSize)
                      .Take(pageSize)
                      .ToList()
                      .AsReadOnly();
        return Task.FromResult((IReadOnlyList<TacomaVenue>)list);
    }

    public Task UpdateAsync(TacomaVenue venue, CancellationToken ct)
    {
        if (!_store.ContainsKey(venue.Id))
            throw new KeyNotFoundException($"Venue {venue.Id} not found.");
        _store[venue.Id] = venue;
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_store.TryRemove(id, out _));
}
