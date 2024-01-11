using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Domain;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityDomain(this IServiceCollection services)
  {
    return services
      .AddSingleton<IRoleManager, RoleManager>()
      .AddSingleton<IRoleSettingsResolver, RoleSettingsResolver>()
      .AddTransient<IUserManager, UserManager>()
      .AddSingleton<IUserSettingsResolver, UserSettingsResolver>();
  }
}
