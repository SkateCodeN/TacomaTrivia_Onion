namespace TacomaTrivia.Application.Models;

using System.ComponentModel.DataAnnotations;
/// <summary>Request DTO for POST.</summary>
public sealed class CreatedVenueRequest
{
    [Required, MinLength(1)]
    public required string Name { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public bool AllowsPets { get; init; }

    [Range(0, 20)]
    public int Rounds { get; init; }
    
    [Range(0, 20)]
    public int TriviaDay { get; init; }
    public  DateTime TriviaStart { get; init; }
    public  string? Website { get; init; }
    public bool AllowsKids { get; init; }
}
