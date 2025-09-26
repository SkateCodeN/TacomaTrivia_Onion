using TacomaTrivia.Application.Models;

namespace TacomaTrivia.Application.Services;

public interface IVenueService
{
    Task<IReadOnlyList<VenueDto>> SearchAsync(string? q, int page, int pageSize, CancellationToken ct);
    Task<VenueDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Guid> CreateAsync(
        string name,
        string? phone,
        string? address,
        int rounds,
        bool allowsPets,
        int triviaDay,
        DateTime triviaStart,
        string? website,
        bool allowsKids,
        CancellationToken ct
    );
    Task UpdateAsync(
        Guid id,
        string name,
        string? phone,
        string? address,
        bool allowsPets,
        int rounds,
        int triviaDay,
        DateTime triviaStart,
        string? website,
        bool allowsKids,
        CancellationToken ct
    );
    Task DeleteAsync(Guid id, CancellationToken ct);
}
