using Event.Domain;
using Event.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Event.Infrastructure;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Domain.Event> Events => Set<Domain.Event>();
    
    public DbSet<Invitation> Invitations => Set<Invitation>();

    public DbSet<Participant> Participants => Set<Participant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new InvitationEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new ParticipantEntityTypeConfigurator());

        base.OnModelCreating(modelBuilder);
    }
}