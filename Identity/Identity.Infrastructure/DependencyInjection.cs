using Common.DI;
using Identity.Contracts.Repositories;
using Identity.Domain;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                               IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        //services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        services.AddDatabaseConfig<AppDbContext>(connectionString);

        services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}