using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Infrastructure.Converters;
using Logitar.Identity.Infrastructure.Passwords;
using Logitar.Identity.Infrastructure.Passwords.Pbkdf2;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityInfrastructure(this IServiceCollection services)
  {
    services.AddLogitarEventSourcingInfrastructure();

    return services
      .AddPasswordStrategies()
      .AddLogitarIdentityDomain()
      .AddSingleton<IEventSerializer>(serviceProvider => new EventSerializer(GetJsonConverters(serviceProvider)))
      .AddSingleton<IPasswordManager, PasswordManager>();
  }

  private static IServiceCollection AddPasswordStrategies(this IServiceCollection services)
  {
    return services.AddTransient<IPasswordStrategy, Pbkdf2Strategy>();
  }

  private static JsonConverter[] GetJsonConverters(IServiceProvider serviceProvider) => new JsonConverter[]
  {
    new PasswordConverter(serviceProvider.GetRequiredService<IPasswordManager>()),
    new PersonNameConverter(),
    new TenantIdConverter(),
    new UniqueNameConverter()
  };
}
