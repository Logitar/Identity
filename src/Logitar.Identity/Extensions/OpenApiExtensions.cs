using Logitar.Identity.Constants;
using Microsoft.OpenApi.Models;

namespace Logitar.Identity;

public static class OpenApiExtensions
{
  public static IServiceCollection AddOpenApi(this IServiceCollection services)
  {
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(config =>
    {
      //config.AddSecurity(); // TODO(fpion): OpenApi Security
      config.SwaggerDoc(name: $"v{Api.Version.Major}", new OpenApiInfo
      {
        Contact = new OpenApiContact
        {
          Email = "francispion@hotmail.com",
          Name = "Logitar Team",
          Url = new Uri("https://github.com/Logitar/Identity", UriKind.Absolute)
        },
        Description = "Identity management platform.",
        License = new OpenApiLicense
        {
          Name = "Use under MIT",
          Url = new Uri("https://github.com/Logitar/Identity/blob/main/LICENSE", UriKind.Absolute)
        },
        Title = Api.Title,
        Version = $"v{Api.Version}"
      });
    });

    return services;
  }

  public static void UseOpenApi(this IApplicationBuilder builder)
  {
    builder.UseSwagger();
    builder.UseSwaggerUI(config => config.SwaggerEndpoint(
      url: $"/swagger/v{Api.Version.Major}/swagger.json",
      name: $"{Api.Title} v{Api.Version}"
    ));
  }

  //private static void AddSecurity(this SwaggerGenOptions options)
  //{
  //  options.AddSecurityDefinition(Schemes.ApiKey, new OpenApiSecurityScheme
  //  {
  //    Description = "Enter your API key in the input below:",
  //    In = ParameterLocation.Header,
  //    Name = Headers.XApiKey,
  //    Scheme = Schemes.ApiKey,
  //    Type = SecuritySchemeType.ApiKey
  //  });
  //  options.AddSecurityRequirement(new OpenApiSecurityRequirement
  //  {
  //    {
  //      new OpenApiSecurityScheme
  //      {
  //        In = ParameterLocation.Header,
  //        Name = Headers.XApiKey,
  //        Reference = new OpenApiReference
  //        {
  //          Id = Schemes.ApiKey,
  //          Type = ReferenceType.SecurityScheme
  //        },
  //        Scheme = Schemes.ApiKey,
  //        Type = SecuritySchemeType.ApiKey
  //      },
  //      new List<string>()
  //    }
  //  });

  //  options.AddSecurityDefinition(Schemes.Basic, new OpenApiSecurityScheme
  //  {
  //    Description = "Enter your credentials in the inputs below:",
  //    In = ParameterLocation.Header,
  //    Name = Headers.Authorization,
  //    Scheme = Schemes.Basic,
  //    Type = SecuritySchemeType.Http
  //  });
  //  options.AddSecurityRequirement(new OpenApiSecurityRequirement
  //  {
  //    {
  //      new OpenApiSecurityScheme
  //      {
  //        In = ParameterLocation.Header,
  //        Name = Headers.Authorization,
  //        Reference = new OpenApiReference
  //        {
  //          Id = Schemes.Basic,
  //          Type = ReferenceType.SecurityScheme
  //        },
  //        Scheme = Schemes.Basic,
  //        Type = SecuritySchemeType.Http
  //      },
  //      new List<string>()
  //    }
  //  });
  //}
}
