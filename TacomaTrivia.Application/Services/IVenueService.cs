using TacomaTrivia.Application.Models;

namespace TacomaTrivia.Application.Services;

public interface IVenueService
{
    Task<IReadOnlyList<VenueDto>> SearchAsync(string? q, int page, int pageSize, CancellationToken ct);
    Task<VenueDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Guid> CreateAsync(
        CreatedVenueRequest createdVenue,
        CancellationToken ct
    );
    Task UpdateAsync(
        Guid id,
        UpdatedVenue updatedVenue,
        CancellationToken ct
    );
    Task DeleteAsync(Guid id, CancellationToken ct);
}
