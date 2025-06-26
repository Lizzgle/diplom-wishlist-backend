using System.Reflection;
using Common.Dropbox.Managers;
using Common.Dropbox.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Wishlist.Contracts.Providers;
using Wishlist.Contracts.Repositories;
using Wishlist.Infrastructure.Providers;
using Wishlist.Infrastructure.Repositories;

namespace Wishlist.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["MongoSettings:ConnectionString"];
        var databaseName = configuration["MongoSettings:DatabaseName"];

        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });

        services.Configure<DropboxOptions>(configuration.GetSection(DropboxOptions.SectionName).Bind);

        services.AddScoped<DropboxTokenManager>();
        
        services.AddScoped<IWishRepository, WishRepository>();
        services.AddScoped<IWishlistRepository, WishlistRepository>();

        services.AddScoped<IFileProvider, FileProvider>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}