using System.Reflection;
using Common;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapperConfig(Assembly.GetExecutingAssembly());
        services.AddMediatRConfig(Assembly.GetExecutingAssembly());

        return services;
    }
}