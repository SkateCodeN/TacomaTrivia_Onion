using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using TacomaTrivia.Domain.AggregateRoots;

namespace TacomaTrivia.Infrastructure.Context;

public sealed class TeamRecordDBContext(DbContextOptions<TeamRecordDBContext> options): DbContext(options)
{
    public DbSet<TeamRecord> TeamRecords => Set<TeamRecord>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasPostgresExtension("pgcrypto");

        //here we basically tell .Net to match these column 
        // names to thos in Postgres
        mb.Entity<TeamRecord>(e =>
        {
            e.ToTable("teamrecords");
            e.HasKey( x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.VenueId).HasColumnName("venueid");
            e.Property(x => x.TeamId).HasColumnName("teamid");
            e.Property(x => x.RecordDate).HasColumnName("recorddate");
            e.Property(x => x.Points).HasColumnName("points");
            e.Property(x => x.Placed).HasColumnName("placed");
            e.Property(x => x.TeamName).HasColumnName("teamname");
        });
    }
}
