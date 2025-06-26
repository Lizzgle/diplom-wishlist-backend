using System.Reflection;
using Common;
using Common.Refit;
using Event.Contracts.HttpClients;
using Microsoft.Extensions.DependencyInjection;

namespace Event.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapperConfig(Assembly.GetExecutingAssembly());
        services.AddMediatRConfig(Assembly.GetExecutingAssembly());

        return services;
    }
}