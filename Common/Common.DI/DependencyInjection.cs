using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Common.DI;

public static class DependencyInjection
{
    /// <summary>
    /// Настройка AutoMapper (можно передавать сборки с мапперами)
    /// </summary>
    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(assemblies));
        return services;
    }

    /// <summary>
    /// Настройка MediatR (можно передавать сборки с обработчиками)
    /// </summary>
    public static IServiceCollection AddMediatRConfig(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        return services;
    }

    /// <summary>
    /// Настройка FluentValidation (можно передавать сборки с валидаторами)
    /// </summary>
    public static IServiceCollection AddFluentValidationConfig(this IServiceCollection services,
                                                               params Assembly[] assemblies)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }

    /// <summary>
    /// Настройка базы данных (DbContext)
    /// </summary>
    public static IServiceCollection AddDatabaseConfig<TContext>(this IServiceCollection services,
                                                                 string connectionString)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(options => options.UseSqlServer(connectionString));
        return services;
    }

    /// <summary>
    /// Настройка Swagger с авторизацией через JWT
    /// </summary>
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

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        return services;
    }
}