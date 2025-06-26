using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Common.Refit;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRefitClientForApi<T>(
        this IServiceCollection services,
        string apiUrl,
        Action<IHttpClientBuilder>? configureClient = null,
        RefitSettings? refitSettings = null) where T : class
    {
        var refitBuilder = services.AddRefitClient<T>(refitSettings).ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri(apiUrl);
        });

        configureClient?.Invoke(refitBuilder);

        return services;

    }
}