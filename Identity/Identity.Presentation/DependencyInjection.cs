using Common.DI;

namespace Identity.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.ConfigureSwagger();
        services.AddFluentValidationConfig();

        services.AddControllers();

        return services;
    }
}