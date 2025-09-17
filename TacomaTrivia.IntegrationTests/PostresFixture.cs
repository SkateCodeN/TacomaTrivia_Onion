//using DotNet.Testcontainers.Builders;
//using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;
using TacomaTrivia.Infrastructure;
using Npgsql;
// and change AppDbContext below if your class name differs

namespace TacomaTrivia.IntegrationTests;

[Parallelizable(ParallelScope.None)]
public abstract class PostgresFixture
{
    //private IContainer _pg = default!;
    private PostgreSqlContainer? _pg;
    private NpgsqlConnection? _conn;
    protected string ConnectionString = default!;

    //responsible for standing up a DB instance w data.
    private Respawner? _respawner;

    protected AppDbContext NewContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options);

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        _pg = new PostgreSqlBuilder()
            .WithDatabase("tacomatrivia")
            .WithUsername("tacomatrivia")
            .WithPassword("testpw")
            .Build();

        await _pg.StartAsync();

        
        ConnectionString = _pg.GetConnectionString();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //Create the context 
        using var ctx = NewContext();
        await ctx.Database.EnsureCreatedAsync();

        _conn = new NpgsqlConnection(ConnectionString);
        await _conn.OpenAsync();

        _respawner = await Respawner.CreateAsync(_conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" }
        });
    }

    [SetUp]
    public Task ResetDb() => _respawner!.ResetAsync(_conn);
    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
         if (_conn is not null)
        {
            await _conn.DisposeAsync();
            _conn = null;
        }
        if (_pg is not null)
        {
            await _pg.DisposeAsync();
            _pg = null;
        }
    }
}
