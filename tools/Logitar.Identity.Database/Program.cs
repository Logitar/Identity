using Logitar.Identity.EntityFrameworkCore.Relational;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.Database;

internal class Program
{
  public static void Main(string[] args)
  {
    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddHostedService<Worker>();

    DatabaseProvider? databaseProvider = builder.Configuration.GetValue<DatabaseProvider?>("DatabaseProvider");
    switch (databaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCorePostgreSQL:
        builder.Services.AddDbContext<IdentityContext>(builder => builder.UseNpgsql(b => b.MigrationsAssembly("Logitar.Identity.EntityFrameworkCore.PostgreSQL")));
        break;
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        builder.Services.AddDbContext<IdentityContext>(builder => builder.UseSqlServer(b => b.MigrationsAssembly("Logitar.Identity.EntityFrameworkCore.SqlServer")));
        break;
    }

    IHost host = builder.Build();
    host.Run();
  }
}
