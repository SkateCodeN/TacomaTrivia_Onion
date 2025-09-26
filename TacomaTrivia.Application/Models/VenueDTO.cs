namespace TacomaTrivia.Application.Models;

/// <summary>Response DTO.</summary>
public sealed record VenueDto(
    Guid Id,
    string Name,
    bool AllowsPets,
    int Rounds,
    string? Phone,
    string? Address,
    int TriviaDay, 
    DateTime TriviaStart, 
    string? Website, 
    bool AllowsKids 
);
