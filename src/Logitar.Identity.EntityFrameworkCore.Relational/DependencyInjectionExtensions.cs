using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.ApiKeys;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Roles;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Repositories;
using Logitar.Identity.Infrastructure;
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
      .AddTransient<IEventBus, EventBus>();
  }

  private static IServiceCollection AddEventHandlers(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyEventHandler, ApiKeyEventHandler>()
      .AddTransient<IRoleEventHandler, RoleEventHandler>()
      .AddTransient<ISessionEventHandler, SessionEventHandler>()
      .AddTransient<IUserEventHandler, UserEventHandler>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyRepository, ApiKeyRepository>()
      .AddTransient<IRoleRepository, RoleRepository>()
      .AddTransient<ISessionRepository, SessionRepository>()
      .AddTransient<IUserRepository, UserRepository>();
  }
}
