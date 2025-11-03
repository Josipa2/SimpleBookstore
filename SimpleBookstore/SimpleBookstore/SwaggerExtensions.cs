using Microsoft.OpenApi.Models;

namespace SimpleBookstore;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "Version .Net 9.0.x",
                Title = "Simple bookstore",
                Description = "Simple bookstore api",
            });

            foreach (var name in Directory.GetFiles(AppContext.BaseDirectory, "*.XML", SearchOption.TopDirectoryOnly))
            {
                c.IncludeXmlComments(filePath: name);
            }

            var securityScheme = new OpenApiSecurityScheme()
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

            var securityRequirement = new OpenApiSecurityRequirement
           {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = "bearerAuth"
                         }
                     },
                     new string[] {}
                 }
             };
            c.AddSecurityDefinition("bearerAuth", securityScheme);
            c.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }
}
