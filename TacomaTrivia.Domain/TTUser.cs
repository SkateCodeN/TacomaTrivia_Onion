namespace TacomaTrivia.Domain;

/// <summary>
/// Domain entity. Invariants live here.
/// </summary>
public sealed class TTUser
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    // For MFA?
    public string? Phone { get; private set; }
    // For Auth and Authorization
    public string? Email { get; private set; }
    
    public string? Role { get; private set; }


    public static TTUser Create
    (
        string name,
        string? phone,
        string? email,
        string? role
    )
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");

        return new TTUser
        {
            Name = name.Trim(),
            Phone = phone,
            Email = email,
            Role = role
        };
    }

    public void Update(
        string name,
        string? phone,
        string? email,
        string? role
    )
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");

        Name = name.Trim();
        Phone = phone;
        Email = email;
        Role = role;
    }
}
