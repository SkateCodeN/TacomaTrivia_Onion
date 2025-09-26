namespace TacomaTrivia.Domain;

/// <summary>
/// Domain entity. Invariants live here.
/// </summary>
public sealed class TacomaVenue
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string? Phone { get; private set; }
    public string? Address { get; private set; }
    public bool AllowsPets { get; private set; }
    public int Rounds { get; private set; }

    public int? TriviaDay { get; private set; }
    public TimeOnly? TriviaStart { get; private set; }

    public string? Website { get; private set; }

    public bool AllowsKids{ get; private set; }


    public static TacomaVenue Create
    (
        string name,
        string? phone,
        string? address,
        bool allowsPets,
        int rounds,
        int? triviaDay,
        TimeOnly? triviaStart,
        string? website,
        bool allowsKids
    )
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
        if (rounds < 0) throw new ArgumentOutOfRangeException(nameof(rounds));
        if (triviaDay < 0 && triviaDay > 7) throw new ArgumentOutOfRangeException(nameof(triviaDay));
        return new TacomaVenue
        {
            Name = name.Trim(),
            Phone = phone,
            Address = address,
            AllowsPets = allowsPets,
            Rounds = rounds,
            TriviaDay = triviaDay,
            TriviaStart = triviaStart,
            Website = website,
            AllowsKids = allowsKids
        };
    }

    public void Update
    (
        string name,
        string? phone,
        string? address,
        bool allowsPets,
        int rounds,
        int? triviaDay,
        TimeOnly? triviaStart,
        string? website,
        bool allowsKids
    )
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
        if (rounds < 0) throw new ArgumentOutOfRangeException(nameof(rounds));
        if (triviaDay < 0 && triviaDay > 7) throw new ArgumentOutOfRangeException(nameof(triviaDay));
        Name = name.Trim();
        Phone = phone;
        Address = address;
        AllowsPets = allowsPets;
        Rounds = rounds;
        TriviaDay = triviaDay;
        TriviaStart = triviaStart;
        Website = website;
        AllowsKids = allowsKids;
    }
}
