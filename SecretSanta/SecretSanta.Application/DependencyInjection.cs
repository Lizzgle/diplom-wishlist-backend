using System.Reflection;
using Common;
using Microsoft.Extensions.DependencyInjection;
using SecretSanta.Application.Providers;
using SecretSanta.Contracts.Providers;

namespace SecretSanta.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapperConfig(Assembly.GetExecutingAssembly());
        services.AddMediatRConfig(Assembly.GetExecutingAssembly());

        services.AddScoped<IDrawLotsProvider, DrawLotsProvider>();

        return services;
    }
}