using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Core;

/// <summary>
/// Defines extension methods for dependency injection.
/// </summary>
public static class DependencyInjectionExtensions
{
  /// <summary>
  /// Registers Identity core services to the specified service collection.
  /// </summary>
  /// <param name="services">The service collection.</param>
  /// <returns>The service collection.</returns>
  public static IServiceCollection AddLogitarIdentityCore(this IServiceCollection services)
  {
    return services
      .AddSingleton<IAddressHelper, AddressHelper>()
      .AddSingleton<IRoleSettingsResolver, RoleSettingsResolver>()
      .AddSingleton<IUserSettingsResolver, UserSettingsResolver>()
      .AddTransient<IRoleManager, RoleManager>()
      .AddTransient<IUserManager, UserManager>();
  }
}
