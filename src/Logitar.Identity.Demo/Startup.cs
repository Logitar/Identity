using Logitar.Identity.Domain.Settings;
using Logitar.Identity.EntityFrameworkCore.SqlServer;

namespace Logitar.Identity.Demo;

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

    services.AddControllers()
      .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    if (_enableOpenApi)
    {
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();
    }

    string connectionString = _configuration.GetValue<string>("SQLCONNSTR_Identity") ?? string.Empty;
    services.AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString);
    services.AddSingleton<IUserSettings>(_configuration.GetSection("User").Get<UserSettings>() ?? new());
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (_enableOpenApi)
    {
      builder.UseSwagger();
      builder.UseSwaggerUI();
    }

    builder.UseHttpsRedirection();

    if (builder is WebApplication application)
    {
      application.MapControllers();
    }
  }
}
