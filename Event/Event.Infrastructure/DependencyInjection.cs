using Common.Refit;
using Core.Api.Options;
using Core.Exceptions;
using Event.Contracts;
using Event.Contracts.HttpClients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Event.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.Configure<UrlOptions>(configuration.GetSection("Url"));
        var urlOptions = configuration.GetRequiredSection(UrlOptions.SectionName).Get<UrlOptions>();
        if (urlOptions is null || urlOptions.IdentityUrl is null)
            throw new AppException("Url options are missing.");
        
        services.AddRefitClientForApi<IIdentityHttpClient>(urlOptions.IdentityUrl);
        
        return services;
    }
}