using Microsoft.EntityFrameworkCore;
using TacomaTrivia.Domain;

namespace TacomaTrivia.Infrastructure;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<TTUser> TTUsers => Set<TTUser>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<TTUser>(e =>
        {
            // OPTIONAL CHANGE: SQL Server default schema is dbo; this keeps names explicit and avoids surprises
            e.ToTable("TTUser", schema: "dbo");

            // OPTIONAL CHANGE: If you want deterministic search behavior, set column collation to CI/AI.
            // WHY: Ensures case/diacritic-insensitive comparisons even if DB default collation changes.
            e.Property(p => p.Name)
             // SQL Server perf tip: constrain length when index/searching
             .HasMaxLength(200)                       
             .UseCollation("Latin1_General_100_CI_AI_SC");
            
        });

    }
}
