using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Tokens;
using Logitar.Identity.Infrastructure.Converters;
using Logitar.Identity.Infrastructure.Passwords;
using Logitar.Identity.Infrastructure.Passwords.Pbkdf2;
using Logitar.Identity.Infrastructure.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingInfrastructure()
      .AddLogitarIdentityDomain()
      .AddPasswordStrategies()
      .AddSingleton<IEventSerializer>(serviceProvider => new EventSerializer(GetJsonConverters(serviceProvider)))
      .AddSingleton<IPasswordManager, PasswordManager>()
      .AddSingleton<PasswordConverter>()
      .AddSingleton(serviceProvider =>
      {
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetSection("Pbkdf2").Get<Pbkdf2Settings>() ?? new();
      })
      .AddTransient<ITokenManager, JsonWebTokenManager>();
  }

  private static IServiceCollection AddPasswordStrategies(this IServiceCollection services)
  {
    return services.AddSingleton<IPasswordStrategy, Pbkdf2Strategy>();
  }

  private static JsonConverter[] GetJsonConverters(IServiceProvider serviceProvider) => new JsonConverter[]
  {
    serviceProvider.GetRequiredService<PasswordConverter>(),
    new ApiKeyIdConverter(),
    new DescriptionConverter(),
    new DisplayNameConverter(),
    new GenderConverter(),
    new LocaleConverter(),
    new PersonNameConverter(),
    new RoleIdConverter(),
    new SessionIdConverter(),
    new TenantIdConverter(),
    new TimeZoneConverter(),
    new UniqueNameConverter(),
    new UrlConverter(),
    new UserIdConverter()
  };
}
