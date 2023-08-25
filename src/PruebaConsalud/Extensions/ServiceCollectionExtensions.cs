using Microsoft.OpenApi.Models;
using PruebaConsalud.Settings;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        AuthenticationSettings settings = new();
        configuration.GetSection(settings.SectionName).Bind(settings);

        services.AddEndpointsMetadataProviderApiExplorer()
        .AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo()
            {
                Description = "PruebaConsalud API documentation",
                Title = "Prueba Consalud",
                Version = "v1",
            });
            s.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
            {
                Description = "Esta es la llave de autenticacion",
                In = ParameterLocation.Header,
                Name = settings.Header,
                Type = SecuritySchemeType.ApiKey,
            });
            var key = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };
            var requirement = new OpenApiSecurityRequirement
                    {
                             { key, new List<string>() }
                    };
            s.AddSecurityRequirement(requirement);
        });

        return services;
    }
}
