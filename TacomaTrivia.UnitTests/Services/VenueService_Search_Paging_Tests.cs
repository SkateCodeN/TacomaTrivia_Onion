using FluentAssertions;
using NUnit.Framework;
using TacomaTrivia.Application.Services;

[TestFixture]
public class VenueService_Search_Paging_Tests
{
    private InMemoryVenueRepository _repo = default!;
    private IVenueService _svc = default!;

    [SetUp]
    public void SetUp()
    {
        _repo = new InMemoryVenueRepository();
        _svc  = new VenueService(_repo);
    }

    private async Task SeedAsync(params (string name,string phone,string addr,int rounds,bool pets)[] rows)
    {
        foreach (var r in rows)
            await _svc.CreateAsync(r.name, r.phone, r.addr, r.rounds, r.pets, default);
    }

    [Test]
    public async Task Search_Should_Be_CaseInsensitive_Across_Name_Phone_Address()
    {
        await SeedAsync(
            ("Alma Mater", "253-555-1234", "1322 Fawcett Ave", 5, true),
            ("Tacoma Taproom", "111-222-3333", "Downtown Tacoma", 3, false),
            ("Alpha", "999-ALPHA", "Somewhere", 2, false)
        );

        (await _svc.SearchAsync("alma", 1, 10, default)).Select(x => x.Name).Should().Contain("Alma Mater");
        (await _svc.SearchAsync("DOWNTOWN", 1, 10, default)).Select(x => x.Name).Should().Contain("Tacoma Taproom");
        (await _svc.SearchAsync("999-alpha", 1, 10, default)).Select(x => x.Name).Should().Contain("Alpha");
    }

    [Test]
    public async Task Pagination_Should_Return_Disjoint_Pages()
    {
        await SeedAsync(
            ("Zebra", "1", "A", 1, false),
            ("Alpha", "2", "B", 2, false),
            ("Tacoma Taproom", "3", "C", 3, false),
            ("Alma Mater", "4", "D", 4, true)
        );

        var p1 = await _svc.SearchAsync(null, 1, 2, default);
        var p2 = await _svc.SearchAsync(null, 2, 2, default);

        p1.Should().HaveCount(2);
        p2.Should().HaveCount(2);
        p1.Select(x => x.Id).Should().NotIntersectWith(p2.Select(y => y.Id));
    }
}
