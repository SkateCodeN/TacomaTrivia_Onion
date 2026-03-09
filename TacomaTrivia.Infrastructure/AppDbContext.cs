using Microsoft.EntityFrameworkCore;
using TacomaTrivia.Domain;

namespace TacomaTrivia.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TacomaVenue> TacomaVenues => Set<TacomaVenue>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasPostgresExtension("pgcrypto");

        b.Entity<TacomaVenue>(e =>
        {
            e.ToTable("venues");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.Phone).HasColumnName("phone");
            e.Property(x => x.Address).HasColumnName("address");
            e.Property(x => x.AllowsPets).HasColumnName("allowspets");
            e.Property(x => x.Rounds).HasColumnName("rounds");
            e.Property(x => x.TriviaDay).HasColumnName("triviaday");
            e.Property(x => x.TriviaStart).HasColumnName("triviastart");
            e.Property(x => x.Website).HasColumnName("website");
            e.Property(x => x.AllowsKids).HasColumnName("allowskids");
        });

        // Optional later:
        // b.UseXminAsConcurrencyToken();
    }
}
