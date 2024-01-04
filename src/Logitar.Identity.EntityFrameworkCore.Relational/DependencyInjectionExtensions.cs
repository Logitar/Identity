using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers;
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
      .AddRepositories();
  }

  private static IServiceCollection AddEventHandlers(this IServiceCollection services)
  {
    return services
      .AddTransient<ISessionEventHandler, SessionEventHandler>()
      .AddTransient<IUserEventHandler, UserEventHandler>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddTransient<ISessionRepository, SessionRepository>()
      .AddTransient<IUserRepository, UserRepository>();
  }
}
