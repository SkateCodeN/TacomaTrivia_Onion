using FluentAssertions;
using NUnit.Framework;
using TacomaTrivia.Domain;
using TacomaTrivia.Infrastructure.Repositories;

namespace TacomaTrivia.IntegrationTests.Infrastructure;

[TestFixture]
[Category("Integration")]
public class EfVenueRepositoryTests : PostgresFixture
{
    [Test]
    public async Task Add_Get_Update_Delete_Roundtrip()
    {
        var repo = new EfVenueRepository(NewContext());

        var v = TacomaVenue.Create("Alma Mater","253-555-1234","1322 Fawcett Ave", true, 5, 1,new TimeOnly(14, 30), "www.test.com", true);
        var id = await repo.AddAsync(v, default);

        (await repo.GetByIdAsync(id, default))!.Name.Should().Be("Alma Mater");

        v.Update("New Name","2","B", false, 6, 6, new TimeOnly(17,30),"www.apothecary.com", false);
        await repo.UpdateAsync(v, default);

        (await repo.GetByIdAsync(id, default))!.Name.Should().Be("New Name");

        (await repo.DeleteAsync(id, default)).Should().BeTrue();
        (await repo.GetByIdAsync(id, default)).Should().BeNull();
    }

    [Test]
    public async Task Search_Uses_Ilike_Substring()
    {
        var repo = new EfVenueRepository(NewContext());
        await repo.AddAsync(TacomaVenue.Create("Alma Mater","p","a", true, 5, 4, new TimeOnly(11,00), "ww", true), default);
        await repo.AddAsync(TacomaVenue.Create("Tacoma Taproom","p","a", false, 3,4, new TimeOnly(11,00), "ww", true), default);

        var hits = await repo.SearchAsync("ALMA", 1, 10, default);
        hits.Select(x => x.Name).Should().Contain("Alma Mater");
    }
}
