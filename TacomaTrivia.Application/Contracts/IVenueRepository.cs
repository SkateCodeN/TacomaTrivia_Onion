using TacomaTrivia.Domain;

namespace TacomaTrivia.Application.Contracts;

/// <summary>Port that Infrastructure implements.</summary>
public interface IVenueRepository
{
    Task<TacomaVenue?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<TacomaVenue>> SearchAsync(string? q, int page, int pageSize, CancellationToken ct);
    Task<Guid> AddAsync(TacomaVenue venue, CancellationToken ct);
    Task UpdateAsync(TacomaVenue venue, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}
