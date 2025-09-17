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
            e.ToTable("tacoma");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.Phone).HasColumnName("phone");
            e.Property(x => x.Address).HasColumnName("address");
            e.Property(x => x.AllowsPets).HasColumnName("allowspets");
            e.Property(x => x.Rounds).HasColumnName("rounds");
        });

        // Optional later:
        // b.UseXminAsConcurrencyToken();
    }
}
