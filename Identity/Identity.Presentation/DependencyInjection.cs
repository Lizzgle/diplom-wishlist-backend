using System.Reflection;
using Common;
using Common.Notification.Implementations;
using Common.Notification.Interfaces;
using Common.Options;
using Identity.Application.Providers;
using Identity.Presentation.Providers;
using Microsoft.OpenApi.Models;

namespace Identity.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSwagger();
        services.AddFluentValidationConfig(Assembly.GetExecutingAssembly());
        services.AddAutoMapperConfig(Assembly.GetExecutingAssembly());
        
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddControllers();

        services.AddScoped<IEmailServiceSender, EmailServiceSender>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        return services;
    }
    
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

           
        });
        return services;
    }
}