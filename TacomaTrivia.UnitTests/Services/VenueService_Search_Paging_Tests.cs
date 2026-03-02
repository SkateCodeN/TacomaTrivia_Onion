using FluentAssertions;
using NUnit.Framework;
using TacomaTrivia.Application.Services;
using TacomaTrivia.Application.Models;

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

    private async Task SeedAsync(
        params (
            string name,
            string phone,
            string addr,
            bool pets,
            int rounds,
            int triviaDay,
            TimeOnly triviaStart,
            string website,
            bool kids
        )[] rows
    )
    {
        foreach (var r in rows)
        {
            var temp = new CreatedVenueRequest
            {
                Name = r.name,
                Phone = r.phone,
                Address = r.addr,
                AllowsPets = r.pets,
                Rounds = r.rounds,
                TriviaDay = r.triviaDay,
                TriviaStart = r.triviaStart,
                Website = r.website,
                AllowsKids = r.kids

            };
            await _svc.CreateAsync(
                temp,
                default
            );
        }
    }

    [Test]
    public async Task Search_Should_Be_CaseInsensitive_Across_Name_Phone_Address()
    {
        await SeedAsync(
            ("Alma Mater", "253-555-1234", "1322 Fawcett Ave", true,5, 3, new TimeOnly(12,0), "www",true),
            ("Tacoma Taproom", "111-222-3333", "Downtown Tacoma", false, 4, 4,new TimeOnly(7,0),"tt" ,false),
            ("Alpha", "999-ALPHA", "Somewhere", false, 5, 4,new TimeOnly(8,0),"ap", false)
        );

        (await _svc.SearchAsync("alma", 1, 10, default)).Select(x => x.Name).Should().Contain("Alma Mater");
        (await _svc.SearchAsync("DOWNTOWN", 1, 10, default)).Select(x => x.Name).Should().Contain("Tacoma Taproom");
        (await _svc.SearchAsync("999-alpha", 1, 10, default)).Select(x => x.Name).Should().Contain("Alpha");
    }

    [Test]
    public async Task Pagination_Should_Return_Disjoint_Pages()
    {
        await SeedAsync(
            ("Zebra", "1", "A", false, 4, 4,new TimeOnly(7,0),"zt",false),
            ("Alpha", "2", "B", true, 6, 7,new TimeOnly(5,30),"ap" , true),
            ("Tacoma Taproom", "3", "C", true, 3, 3,new TimeOnly(9,30),"tt" , false),
            ("Alma Mater", "4", "D", true, 7, 7,new TimeOnly(10,10),"am" , true)
        );

        var p1 = await _svc.SearchAsync(null, 1, 2, default);
        var p2 = await _svc.SearchAsync(null, 2, 2, default);

        p1.Should().HaveCount(2);
        p2.Should().HaveCount(2);
        p1.Select(x => x.Id).Should().NotIntersectWith(p2.Select(y => y.Id));
    }
}
