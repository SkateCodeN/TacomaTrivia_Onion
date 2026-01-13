using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using TacomaTrivia.Domain.AggregateRoots.Teams;

namespace TacomaTrivia.Infrastructure.Context;

public sealed class TeamDbContext(DbContextOptions<TeamDbContext> options): DbContext(options)
{
    public DbSet<TriviaTeam> TriviaTeams => Set<TriviaTeam>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasPostgresExtension("pgcrypto");

        //here we basically tell .Net to match these column 
        // names to thos in Postgres
        mb.Entity<TriviaTeam>(team =>
        {
            team.ToTable("teams");
            team.HasKey( x => x.Id);
            team.Property(x => x.Id).HasColumnName("id");
            team.Property(x => x.Name).HasColumnName("name").IsRequired();
            team.Property(x => x.DateCreated).HasColumnName("datecreated");
            team.Property(x => x.TeamOwnerId).HasColumnName("teamownerid");

            // We user Navigation() to configure the relationship with a backing field
            team.Navigation(t => t.Members)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_members");
            // We are telling Ef core that the Teams table is to be linked to the
            // TriviaTeam class, and that this team has many team members in the _member property
            team.HasMany<TeamMember>(t => t.Members)
                .WithOne()
                .HasForeignKey(member => member.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // This tells EF Core how to map existing values to the team_members table.
        mb.Entity<TeamMember>(member =>
        {
            member.ToTable("team_members");
            member.HasKey(m => new {m.TeamId, m.UserId});
            member.Property(m => m.UserId).HasColumnName("userid");
            member.Property(m => m.TeamId).HasColumnName("teamid");
            member.Property(m => m.Role).HasColumnName("role").IsRequired();
        });
    }
}
