using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Domain;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityDomain(this IServiceCollection services)
  {
    return services.AddTransient<IUserManager, UserManager>();
  }
}
