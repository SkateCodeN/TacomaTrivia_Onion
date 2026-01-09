using TacomaTrivia.Domain.AggregateRoots;
using TacomaTrivia.Application.Contracts.TeamRecords;
using TacomaTrivia.Application.Models.TeamRecords;



namespace TacomaTrivia.Application.Services.TeamRecords;

public sealed class TeamRecordSvc(ITeamRecordRepository repo) : ITeamRecordSvc
{
    private readonly ITeamRecordRepository _repo = repo;

    public async Task<IReadOnlyList<TeamRecordDTO>> SearchAsync(string? q, int page, int size, CancellationToken ct)
    {
        var items = await _repo.SearchAsync(q, page, size, ct);
        return items.Select(
            v => new TeamRecordDTO(
                v.Id,
                v.VenueId,
                v.TeamId,
                v.RecordDate,
                v.TeamName,
                v.Points,
                v.Placed 
            )
        ).ToList();
    }

    public async Task<TeamRecordDTO?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if(id == Guid.Empty) throw new ArgumentException ("Id cannot be empty");
        var v = await _repo.GetByIdAsync(id, ct);
        return v is null ? null :
        new TeamRecordDTO(
            v.Id,
            v.VenueId,
            v.TeamId,
            v.RecordDate,
            v.TeamName,
            v.Points,
            v.Placed 
        );
    }

    public async Task<Guid> CreateAsync(
        Guid venueId,
        Guid teamId,
        DateOnly recordDate,
        string teamName,
        int? points,
        int? placed,
        CancellationToken ct

    ) => await _repo.AddAsync(
        TeamRecord.Create(
            teamName,
            recordDate,
            points,
            placed,
            venueId,
            teamId
        ),
        ct
    );

    public async Task UpdateAsync(
        Guid id,
        Guid venueId,
        Guid teamId,
        DateOnly recordDate,
        string teamName,
        int? points,
        int? placed,
        CancellationToken ct
    )
    {
        if(id == Guid.Empty) throw new ArgumentException("Id can't be no empty");
        var record = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException($"Venue {id} not found");
        record.Update(
            teamName,
            recordDate,
            points,
            placed,
            venueId,
            teamId
        );
        await _repo.UpdateAsync(record, ct);

    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
{
    if(id == Guid.Empty) throw new ArgumentException("Id cannot be empty");
    var deleted = await _repo.DeleteAsync(id, ct);
    if(!deleted) throw new KeyNotFoundException($"Venue {id} not found");
}
}

