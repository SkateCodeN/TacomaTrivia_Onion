using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TacomaTrivia.Infrastructure;
using TacomaTrivia.Api;

namespace TacomaTrivia.IntegrationTests.Api;

[TestFixture, Category("Integration")]
public class VenuesApiTests : PostgresFixture
{
    private sealed class TestsAppFactory : WebApplicationFactory<Program>
    {
        private readonly string _cs;
        public TestsAppFactory(string cs) => _cs = cs;

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.ConfigureServices(services =>
            {
                var desc = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                services.Remove(desc);
                services.AddDbContext <AppDbContext>(opt => opt.UseNpgsql(_cs));
            }
        );

    }

    [Test]
    public async Task Post_then_GetById_Should_Be_201_then_200()
    {
        await using var app = new TestsAppFactory(ConnectionString);
        var clinet = app.CreateClient();

        var payload = new
        {
            name = "Alma Mater",
            phone = "253-555-1234",
            address = "1332 Fawcett Ave",
            rounds = 5,
            allowsPets = true
        };
        var post = await clinet.PostAsJsonAsync("/api/venues", payload);
        Assert.That(post.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var location = post.Headers.Location;

        // we dont have a getAsync functiion in our repository
        var get = await clinet.GetAsync(location);
        Assert.That(get.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Put_with_bad_payload_should_return_400()
    {
        await using var app = new TestsAppFactory(ConnectionString);
        var client = app.CreateClient();

        var id = Guid.NewGuid();
        var bad = new
        {
            name = "",
            phone = "x",
            address = "y",
            rounds = -1,
            allowsPets = true
        };
        var put = await client.PutAsJsonAsync($"/api/venues/{id}", bad);
        Assert.That(put.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}