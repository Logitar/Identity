using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Tokens;
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
      .AddLogitarIdentityCore()
      .AddPasswordStrategies()
      .AddSingleton(serviceProvider =>
      {
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetSection(Pbkdf2Settings.SectionKey).Get<Pbkdf2Settings>() ?? new();
      })
      .AddSingleton<IEventSerializer, EventSerializer>()
      .AddSingleton<IPasswordManager, PasswordManager>()
      .AddSingleton<PasswordConverter>()
      .AddScoped<IEventBus, EventBus>()
      .AddScoped<ITokenManager, JsonWebTokenManager>();
  }

  private static IServiceCollection AddPasswordStrategies(this IServiceCollection services)
  {
    return services.AddSingleton<IPasswordStrategy, Pbkdf2Strategy>();
  }
}
