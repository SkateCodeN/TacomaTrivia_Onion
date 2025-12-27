namespace TacomaTrivia.Domain.AggregateRoots;

public sealed class TeamRecord
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid VenueId { get; private set; }

    public Guid TeamId { get; private set; }

    public DateOnly RecordDate { get; private set; }

    public int? Points { get; private set; }

    public int? Placed { get; private set;}

    public string TeamName { get; private set; } = default!;

    // Business rule:
    // A Team Record can be created
    public static TeamRecord Create
    (
        string teamName,
        DateOnly date,
        int? points,
        int? placed,
        Guid venueId,
        Guid teamId

    )
    {
        // we check for non negative points and ditto for place
        if (points < 0 ) throw new ArgumentOutOfRangeException(nameof(points));

        //
        if (placed <= 0) throw new ArgumentOutOfRangeException(nameof(placed));
        return new TeamRecord
        {
            TeamName = teamName.Trim(),
            RecordDate = date,
            Points = points,
            Placed = placed,
            VenueId = venueId,
            TeamId = teamId
        };
    }

    // Business rule:
    // A Team Record can be updated
    public void Update
    (
        string teamName,
        DateOnly date,
        int? points,
        int? placed,
        Guid venueId,
        Guid teamId
    )
    {
        if (string.IsNullOrEmpty(teamName)) 
            throw new ArgumentException("A team name is required");
        
         TeamName = teamName.Trim();
            RecordDate = date;
            Points = points;
            Placed = placed;
            VenueId = venueId;
            TeamId = teamId;
        
    }

}

