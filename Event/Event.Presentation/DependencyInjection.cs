using System.Reflection;
using Common;
using Core.Api.Options;
using Microsoft.OpenApi.Models;

namespace Event.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSwagger();
        services.AddFluentValidationConfig(Assembly.GetExecutingAssembly());
        services.AddAutoMapperConfig(Assembly.GetExecutingAssembly());
        services.AddCorsPolicy();
        
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddControllers();

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

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        return services;
    }
}