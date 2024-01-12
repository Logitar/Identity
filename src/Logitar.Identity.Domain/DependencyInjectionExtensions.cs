using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Domain;

/// <summary>
/// Defines extension methods for dependency injection.
/// </summary>
public static class DependencyInjectionExtensions
{
  /// <summary>
  /// Registers Identity domain services to the specified service collection.
  /// </summary>
  /// <param name="services">The service collection.</param>
  /// <returns>The service collection.</returns>
  public static IServiceCollection AddLogitarIdentityDomain(this IServiceCollection services)
  {
    return services
      .AddSingleton<IRoleSettingsResolver, RoleSettingsResolver>()
      .AddSingleton<IUserSettingsResolver, UserSettingsResolver>()
      .AddTransient<IRoleManager, RoleManager>()
      .AddTransient<IUserManager, UserManager>();
  }
}
