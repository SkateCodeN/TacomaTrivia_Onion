using FluentAssertions;
using NUnit.Framework;
using TacomaTrivia.Application.Services;
using TacomaTrivia.Application.Models;

[TestFixture]
public class VenueService_Create_Get_Tests
{
    private InMemoryVenueRepository _repo = default!;
    private IVenueService _svc = default!;

    [SetUp]
    public void SetUp()
    {
        _repo = new InMemoryVenueRepository();
        _svc  = new VenueService(_repo);
    }

    [Test]
    public async Task CreateAsync_ShouldReturn_Id_AndPersistedVenue()
    {
        var data = new CreatedVenueRequest
        {
            Name = "Alma Mater",
            Phone = "253-555-1234",
            Address = "1322 Fawcett Ave",
            AllowsPets = true,
            Rounds = 5,
            TriviaDay = 5,
            TriviaStart = new TimeOnly(10, 30),
            Website = "www.alma.com",
            AllowsKids = true

        };
        var id = await _svc.CreateAsync(
            data,
            ct: default
        );

        id.Should().NotBeEmpty();

        var dto = await _svc.GetByIdAsync(id, default);
        dto.Should().NotBeNull();
        dto!.Name.Should().Be("Alma Mater");
        dto.Phone.Should().Be("253-555-1234");
        dto.Address.Should().Be("1322 Fawcett Ave");
        dto.Rounds.Should().Be(5);
        dto.AllowsPets.Should().BeTrue();
    }
}
