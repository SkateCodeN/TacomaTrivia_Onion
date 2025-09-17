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

        var v = TacomaVenue.Create("Alma Mater","253-555-1234","1322 Fawcett Ave", true, 5);
        var id = await repo.AddAsync(v, default);

        (await repo.GetByIdAsync(id, default))!.Name.Should().Be("Alma Mater");

        v.Update("New Name","2","B", false, 6);
        await repo.UpdateAsync(v, default);

        (await repo.GetByIdAsync(id, default))!.Name.Should().Be("New Name");

        (await repo.DeleteAsync(id, default)).Should().BeTrue();
        (await repo.GetByIdAsync(id, default)).Should().BeNull();
    }

    [Test]
    public async Task Search_Uses_Ilike_Substring()
    {
        var repo = new EfVenueRepository(NewContext());
        await repo.AddAsync(TacomaVenue.Create("Alma Mater","p","a", true, 5), default);
        await repo.AddAsync(TacomaVenue.Create("Tacoma Taproom","p","a", false, 3), default);

        var hits = await repo.SearchAsync("ALMA", 1, 10, default);
        hits.Select(x => x.Name).Should().Contain("Alma Mater");
    }
}
