using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Tokens;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.CustomAttributes;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers;
using Logitar.Identity.EntityFrameworkCore.Relational.Repositories;
using Logitar.Identity.EntityFrameworkCore.Relational.Tokens;
using Logitar.Identity.Infrastructure;
using Logitar.Identity.Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityWithEntityFrameworkCoreRelational(this IServiceCollection services)
  {
    return services
      .AddEventHandlers()
      .AddLogitarEventSourcingWithEntityFrameworkCoreRelational()
      .AddLogitarIdentityInfrastructure()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddRepositories()
      .AddTransient<ICustomAttributeService, CustomAttributeService>()
      .AddTransient<ITokenBlacklist, TokenBlacklist>();
  }

  private static IServiceCollection AddEventHandlers(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyEventHandler, ApiKeyEventHandler>()
      .AddTransient<IOneTimePasswordEventHandler, OneTimePasswordEventHandler>()
      .AddTransient<IRoleEventHandler, RoleEventHandler>()
      .AddTransient<ISessionEventHandler, SessionEventHandler>()
      .AddTransient<IUserEventHandler, UserEventHandler>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyRepository, ApiKeyRepository>()
      .AddTransient<IOneTimePasswordRepository, OneTimePasswordRepository>()
      .AddTransient<IRoleRepository, RoleRepository>()
      .AddTransient<ISessionRepository, SessionRepository>()
      .AddTransient<IUserRepository, UserRepository>();
  }
}
