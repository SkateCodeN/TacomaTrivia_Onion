using FluentAssertions;
using NUnit.Framework;
using TacomaTrivia.Application.Models;
using TacomaTrivia.Application.Services;

[TestFixture]
public class VenueService_Update_Delete_Tests
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
    public async Task Update_Should_Mutate_Existing_Venue()
    {
        var id = await _svc.CreateAsync("Old", "1", "A", 2, false, default);

        var patch = new UpdatedVenue
        {
            Name = "New",
            Phone = "2",
            Address = "B",
            AllowsPets = true,
            Rounds = 6
        };

        await _svc.UpdateAsync(
            id,
            patch.Name,
            patch.Phone,
            patch.Address,
            patch.AllowsPets,
            patch.Rounds,
            default
        );

        var dto = await _svc.GetByIdAsync(id, default);
        dto!.Name.Should().Be("New");
        dto.Phone.Should().Be("2");
        dto.Address.Should().Be("B");
        dto.Rounds.Should().Be(6);
        dto.AllowsPets.Should().BeTrue();
    }

    [Test]
    public async Task Delete_Should_Remove_And_GetById_Returns_Null()
    {
        var id = await _svc.CreateAsync("X", "p", "a", 1, false, default);

        await _svc.DeleteAsync(id, default); // void + throw-on-not-found policy

        var dto = await _svc.GetByIdAsync(id, default);
        dto.Should().BeNull();
    }

    [Test]
    public async Task Delete_NotFound_Should_Throw()
    {
        var act = () => _svc.DeleteAsync(Guid.NewGuid(), default);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
