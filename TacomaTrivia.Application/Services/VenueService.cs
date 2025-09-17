using TacomaTrivia.Application.Contracts;
using TacomaTrivia.Application.Models;
using TacomaTrivia.Domain;

namespace TacomaTrivia.Application.Services;

/// <summary>Application/use-case layer.</summary>
public sealed class VenueService(IVenueRepository repo) : IVenueService
{
    private readonly IVenueRepository _repo = repo;

    public async Task<IReadOnlyList<VenueDto>> SearchAsync(string? q, int page, int size, CancellationToken ct)
    {
        var items = await _repo.SearchAsync(q, page, size, ct);
        return items.Select(v => new VenueDto(v.Id, v.Name, v.AllowsPets, v.Rounds, v.Phone, v.Address)).ToList();
    }

    public async Task<VenueDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty");
        var v = await _repo.GetByIdAsync(id, ct);
        return v is null ? null : new VenueDto(v.Id, v.Name, v.AllowsPets, v.Rounds, v.Phone, v.Address);
    }

    public Task<Guid> CreateAsync(string name, string? phone, string? address, int rounds, bool allowsPets, CancellationToken ct)
        => _repo.AddAsync(TacomaVenue.Create(name, phone, address, allowsPets, rounds), ct);

    public async Task UpdateAsync(Guid id, string name, string? phone, string? address, bool allowsPets, int rounds, CancellationToken ct)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty");
        var venue = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException($"Venue {id} not found");
        venue.Update(name, phone, address, allowsPets, rounds);
        await _repo.UpdateAsync(venue, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.");
        var deleted = await _repo.DeleteAsync(id, ct);
        if (!deleted) throw new KeyNotFoundException($"Venue {id} not found");
    }
}
