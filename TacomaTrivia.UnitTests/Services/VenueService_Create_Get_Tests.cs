using FluentAssertions;
using NUnit.Framework;
using TacomaTrivia.Application.Services;

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
        var id = await _svc.CreateAsync(
            name: "Alma Mater",
            phone: "253-555-1234",
            address: "1322 Fawcett Ave",
            rounds: 5,
            allowsPets: true,
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
