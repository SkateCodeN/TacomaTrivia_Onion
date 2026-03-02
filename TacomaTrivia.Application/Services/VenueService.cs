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
        return items.Select(
            v => new VenueDto(
                v.Id,
                v.Name,
                v.AllowsPets,
                v.Rounds,
                v.Phone,
                v.Address,
                v.TriviaDay, 
                v.TriviaStart, 
                v.Website, 
                v.AllowsKids 
            )
        ).ToList();
    }

    public async Task<VenueDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty");
        var v = await _repo.GetByIdAsync(id, ct);
        return v is null ? null :
            new VenueDto(
                v.Id,
                v.Name,
                v.AllowsPets,
                v.Rounds,
                v.Phone,
                v.Address,
                v.TriviaDay, 
                v.TriviaStart, 
                v.Website, 
                v.AllowsKids 
            );
    }

    public Task<Guid> CreateAsync(
        CreatedVenueRequest createdVenue,
        CancellationToken ct
    )
        => _repo.AddAsync(
            TacomaVenue.Create(
                createdVenue.Name,
                createdVenue.Phone,
                createdVenue.Address,
                createdVenue.AllowsPets,
                createdVenue.Rounds,
                createdVenue.TriviaDay,
                createdVenue.TriviaStart,
                createdVenue.Website,
                createdVenue.AllowsKids
            ),
        ct);

    public async Task UpdateAsync(
        Guid id,
        UpdatedVenue updatedVenue,
        CancellationToken ct
    )
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty");
        var venue = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException($"Venue {id} not found");
        venue.Update(
            updatedVenue.Name,
            updatedVenue.Phone,
            updatedVenue.Address,
            updatedVenue.AllowsPets,
            updatedVenue.Rounds,
            updatedVenue.TriviaDay,
            updatedVenue.TriviaStart,
            updatedVenue.Website,
            updatedVenue.AllowsKids
        );
        await _repo.UpdateAsync(venue, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.");
        var deleted = await _repo.DeleteAsync(id, ct);
        if (!deleted) throw new KeyNotFoundException($"Venue {id} not found");
    }
}
