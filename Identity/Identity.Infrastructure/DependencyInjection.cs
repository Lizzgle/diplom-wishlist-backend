using Identity.Contracts.Repositories;
using Identity.Contracts.Services;
using Identity.Domain;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                               IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // services.AddDatabaseConfig<AppDbContext>(connectionString);
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddUserManager<UserManager<User>>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddDefaultTokenProviders();
        
        //services.AddScoped<IUserStore<User>, UserStore<User, IdentityRole, AppDbContext>>();
        //services.AddScoped<IUserEmailStore<User>>(sp => (IUserEmailStore<User>)sp.GetRequiredService<IUserStore<User>>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();

        services.AddScoped<IRedisService, RedisService>();
        
        return services;
    }
}