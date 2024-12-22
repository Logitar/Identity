using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Tokens;
using Logitar.Identity.Core.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Repositories;
using Logitar.Identity.EntityFrameworkCore.Relational.Tokens;
using Logitar.Identity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityWithEntityFrameworkCoreRelational(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingWithEntityFrameworkCoreRelational()
      .AddLogitarIdentityInfrastructure()
      .AddEventHandlers()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddRepositories()
      //.AddTransient<ICustomAttributeService, CustomAttributeService>() // TODO(fpion): implement
      .AddScoped<ITokenBlacklist, TokenBlacklist>();
  }

  private static IServiceCollection AddEventHandlers(this IServiceCollection services)
  {
    return services/*
      .AddTransient<IApiKeyEventHandler, ApiKeyEventHandler>()
      .AddTransient<IOneTimePasswordEventHandler, OneTimePasswordEventHandler>()
      .AddTransient<IRoleEventHandler, RoleEventHandler>()
      .AddTransient<ISessionEventHandler, SessionEventHandler>()
      .AddTransient<IUserEventHandler, UserEventHandler>()*/; // TODO(fpion): implement
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddScoped<IApiKeyRepository, ApiKeyRepository>()
      .AddScoped<IOneTimePasswordRepository, OneTimePasswordRepository>()
      .AddScoped<IRoleRepository, RoleRepository>()
      .AddScoped<ISessionRepository, SessionRepository>()
      .AddScoped<IUserRepository, UserRepository>();
  }
}
