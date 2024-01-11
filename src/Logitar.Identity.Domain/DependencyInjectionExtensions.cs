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
      .AddSingleton<IRoleSettingsResolver, RoleSettingsResolver>()
      .AddSingleton<IUserSettingsResolver, UserSettingsResolver>()
      .AddTransient<IRoleManager, RoleManager>()
      .AddTransient<IUserManager, UserManager>();
  }
}
