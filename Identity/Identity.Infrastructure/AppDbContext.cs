using Identity.Domain;
using Identity.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;

public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<User> Users { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new RoleEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new FriendRequestEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new FriendshipEntityTypeConfigurator());
    }
}