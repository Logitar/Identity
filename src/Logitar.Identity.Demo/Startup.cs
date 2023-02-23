using Logitar.Identity.EntityFrameworkCore.PostgreSQL;
using System.Text.Json.Serialization;

namespace Logitar.Identity.Demo;

internal class Startup : StartupBase
{
  private readonly IConfiguration _configuration;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    services.AddControllers()
      .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    services.AddMemoryCache();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddLogitarIdentity();

    string connectionString = _configuration.GetValue<string>("POSTGRESQLCONNSTR_IdentityContext") ?? string.Empty;
    services.AddLogitarIdentityWithEntityFrameworkCorePostgreSQL(connectionString);

    services.AddSingleton<ICurrentActor, HttpCurrentActor>();
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (builder is WebApplication application)
    {
      if (application.Environment.IsDevelopment())
      {
        application.UseSwagger();
        application.UseSwaggerUI();
      }

      application.UseHttpsRedirection();
      application.MapControllers();
    }
  }
}
