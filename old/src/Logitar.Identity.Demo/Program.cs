using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.Demo;

public class Program
{
  public static async Task Main(string[] args)
  {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    Startup startup = new(builder.Configuration);
    startup.ConfigureServices(builder.Services);

    WebApplication application = builder.Build();

    if (builder.Configuration.GetValue<bool>("EnableMigrations"))
    {
      using IServiceScope scope = application.Services.CreateScope();

      using EventContext eventContext = scope.ServiceProvider.GetRequiredService<EventContext>();
      await eventContext.Database.MigrateAsync();

      using IdentityContext identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
      await identityContext.Database.MigrateAsync();
    }

    startup.Configure(application);

    application.Run();
  }
}
