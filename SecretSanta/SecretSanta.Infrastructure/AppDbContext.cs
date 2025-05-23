using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain;
using SecretSanta.Infrastructure.Configurations;

namespace SecretSanta.Infrastructure;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    
    public DbSet<Player> Players => Set<Player>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GameEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}