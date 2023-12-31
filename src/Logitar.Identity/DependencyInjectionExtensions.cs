﻿using Logitar.Identity.ApiKeys;
using Logitar.Identity.Realms;
using Logitar.Identity.Roles;
using Logitar.Identity.Sessions;
using Logitar.Identity.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Logitar.Identity;

/// <summary>
/// Provides extension methods to use the identity system.
/// </summary>
public static class DependencyInjectionExtensions
{
  /// <summary>
  /// Registers the required dependencies of the identity system to the specified service collection.
  /// </summary>
  /// <param name="services">The service collection.</param>
  /// <returns>The service collection.</returns>
  public static IServiceCollection AddLogitarIdentity(this IServiceCollection services)
  {
    return services.AddLogitarIdentity<RequestPipeline>();
  }

  /// <summary>
  /// Registers the required dependencies of the identity system to the specified service collection.
  /// </summary>
  /// <typeparam name="TRequestPipeline">The type of the request pipeline.</typeparam>
  /// <param name="services">The service collection.</param>
  /// <returns>The service collection.</returns>
  public static IServiceCollection AddLogitarIdentity<TRequestPipeline>(this IServiceCollection services)
    where TRequestPipeline : class, IRequestPipeline
  {
    Assembly assembly = typeof(DependencyInjectionExtensions).Assembly;

    return services
      .AddFacades()
      .AddMediatR(config => config.RegisterServicesFromAssembly(assembly))
      .AddSingleton<ICacheService, CacheService>()
      .AddTransient<IApiKeyHelper, ApiKeyHelper>()
      .AddTransient<IRequestPipeline, TRequestPipeline>()
      .AddTransient<ISessionHelper, SessionHelper>()
      .AddTransient<IUserHelper, UserHelper>();

    // TODO(fpion): ICurrentActor
    // TODO(fpion): IMemoryCache
  }

  /// <summary>
  /// Registers the service facades to the specified service collection.
  /// </summary>
  /// <param name="services">The service collection.</param>
  /// <returns>The service collection.</returns>
  private static IServiceCollection AddFacades(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyService, ApiKeyService>()
      .AddTransient<IRealmService, RealmService>()
      .AddTransient<IRoleService, RoleService>()
      .AddTransient<ISessionService, SessionService>()
      .AddTransient<IUserService, UserService>();
  }
}
