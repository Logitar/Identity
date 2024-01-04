﻿using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Application.Account;
using Logitar.Identity.Application.Caching;
using Logitar.Identity.Application.Sessions;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Queriers;
using Logitar.Identity.EntityFrameworkCore.SqlServer;
using Logitar.Identity.Filters;
using Logitar.Identity.Infrastructure;
using Logitar.Identity.Infrastructure.Caching;
using Logitar.Identity.Settings;

namespace Logitar.Identity;

internal class Startup : StartupBase
{
  private readonly IConfiguration _configuration;
  private readonly bool _enableOpenApi;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
    _enableOpenApi = configuration.GetValue<bool>("EnableOpenApi");
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    services.AddControllers(options => options.Filters.Add<IdentityExceptionFilter>())
      .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    CorsSettings corsSettings = _configuration.GetSection("Cors").Get<CorsSettings>() ?? new();
    services.AddSingleton(corsSettings);
    services.AddCors(corsSettings);

    UserSettings userSettings = _configuration.GetSection("User").Get<UserSettings>() ?? new();
    services.AddSingleton<IUserSettings>(userSettings);

    // TODO(fpion): Authentication
    // TODO(fpion): Authorization

    CookiesSettings cookiesSettings = _configuration.GetSection("Cookies").Get<CookiesSettings>() ?? new();
    services.AddSingleton(cookiesSettings);
    services.AddSession(options =>
    {
      options.Cookie.SameSite = cookiesSettings.Session.SameSite;
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

    services.AddApplicationInsightsTelemetry();
    IHealthChecksBuilder healthChecks = services.AddHealthChecks();

    if (_enableOpenApi)
    {
      services.AddOpenApi();
    }

    services.AddDistributedMemoryCache();
    services.AddLogitarIdentityDomain();
    services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    services.AddMemoryCache();
    services.AddSingleton<ICacheService, CacheService>();
    services.AddTransient<IAccountService, AccountService>();
    services.AddTransient<IActorService, ActorService>();
    services.AddTransient<IEventBus, EventBus>();
    services.AddTransient<ISessionQuerier, SessionQuerier>();

    string connectionString;
    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider")
      ?? DatabaseProvider.EntityFrameworkCoreSqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        connectionString = _configuration.GetValue<string>("SQLCONNSTR_Identity") ?? string.Empty;
        services.AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString);
        healthChecks.AddDbContextCheck<EventContext>();
        healthChecks.AddDbContextCheck<IdentityContext>();
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (_enableOpenApi)
    {
      builder.UseSwagger();
      builder.UseSwaggerUI();
    }

    builder.UseHttpsRedirection();
    builder.UseCors();
    builder.UseSession();
    builder.UseAuthentication();
    builder.UseAuthorization();

    if (builder is WebApplication application)
    {
      application.MapControllers();
      application.MapHealthChecks("/health");
    }
  }
}
