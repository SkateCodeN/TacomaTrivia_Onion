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
        var data = new CreatedVenueRequest
        {
            Name = "Old",
            Phone = "1",
            Address = "A",
            AllowsPets = true,
            Rounds = 2,
            TriviaDay = 3,
            Website = "dofoo.com",
            AllowsKids = true
        };
        var id = await _svc.CreateAsync(
            data,
            default
        );
       
        var patch2 = new UpdatedVenue
        {
            Name = "New",
            Phone = "2",
            Address = "B",
            AllowsPets = true,
            Rounds = 6,
            TriviaDay = 4,
            TriviaStart = new TimeOnly(4, 30),
            Website = "new.com",
            AllowsKids = true
            

        };
        // var patch = new UpdatedVenue
        // {
        //     Name = "New",
        //     Phone = "2",
        //     Address = "B",
        //     AllowsPets = true,
        //     Rounds = 6
        // };

        await _svc.UpdateAsync(
            id,
            patch2,
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
        var data = new CreatedVenueRequest
        {
            Name = "X",
            Phone = "P",
            Address = "A",
            AllowsPets = false,
            Rounds = 1,
            TriviaDay = 3,
            Website = "dofoo.com",
            AllowsKids = true
        };
        var id = await _svc.CreateAsync(data, default);

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
