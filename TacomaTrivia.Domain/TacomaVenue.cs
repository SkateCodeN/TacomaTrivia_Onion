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

    public static TacomaVenue Create(string name, string? phone, string? address, bool allowsPets, int rounds)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
        if (rounds < 0) throw new ArgumentOutOfRangeException(nameof(rounds));
        return new TacomaVenue { Name = name.Trim(), Phone = phone, Address = address, AllowsPets = allowsPets, Rounds = rounds };
    }

    public void Update(string name, string? phone, string? address, bool allowsPets, int rounds)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
        if (rounds < 0) throw new ArgumentOutOfRangeException(nameof(rounds));
        Name = name.Trim(); Phone = phone; Address = address; AllowsPets = allowsPets; Rounds = rounds;
    }
}
