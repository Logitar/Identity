using Bogus;
using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityDb = Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

namespace Logitar.Identity;

public abstract class IntegrationTests : IAsyncLifetime
{
  private readonly DatabaseProvider _databaseProvider;

  protected Faker Faker { get; } = new();

  protected IConfiguration Configuration { get; }
  protected IServiceProvider ServiceProvider { get; }

  protected IdentityContext IdentityContext { get; }

  protected IntegrationTests(DatabaseProvider databaseProvider)
  {
    _databaseProvider = databaseProvider;

    Configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .Build();

    ServiceCollection services = new();
    services.AddSingleton(Configuration);

    string connectionString = Configuration.GetConnectionString(databaseProvider.ToString())
      ?.Replace("{Database}", GetType().Name)
      ?? throw new InvalidOperationException($"The connection string '{databaseProvider}' is required.");
    switch (databaseProvider)
    {
      case DatabaseProvider.PostgreSQL:
        services.AddLogitarIdentityWithEntityFrameworkCorePostgreSQL(connectionString);
        break;
      case DatabaseProvider.SqlServer:
        services.AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString);
        break;
    }

    ServiceProvider = services.BuildServiceProvider();

    IdentityContext = ServiceProvider.GetRequiredService<IdentityContext>();
  }

  public async Task InitializeAsync()
  {
    EventContext eventContext = ServiceProvider.GetRequiredService<EventContext>();
    await eventContext.Database.MigrateAsync();

    await IdentityContext.Database.MigrateAsync();

    StringBuilder sql = new();
    sql.Append(GetDeleteBuilder(IdentityDb.TokenBlacklist.Table).Build().Text).Append(';').AppendLine();
    sql.Append(GetDeleteBuilder(IdentityDb.CustomAttributes.Table).Build().Text).Append(';').AppendLine();
    sql.Append(GetDeleteBuilder(IdentityDb.OneTimePasswords.Table).Build().Text).Append(';').AppendLine();
    sql.Append(GetDeleteBuilder(IdentityDb.Sessions.Table).Build().Text).Append(';').AppendLine();
    sql.Append(GetDeleteBuilder(IdentityDb.Users.Table).Build().Text).Append(';').AppendLine();
    sql.Append(GetDeleteBuilder(IdentityDb.ApiKeys.Table).Build().Text).Append(';').AppendLine();
    sql.Append(GetDeleteBuilder(IdentityDb.Roles.Table).Build().Text).Append(';').AppendLine();
    sql.Append(GetDeleteBuilder(IdentityDb.Actors.Table).Build().Text).Append(';').AppendLine();
    sql.Append(GetDeleteBuilder(EventDb.Streams.Table).Build().Text).Append(';').AppendLine();

    await IdentityContext.Database.ExecuteSqlRawAsync(sql.ToString());
  }
  protected virtual IDeleteBuilder GetDeleteBuilder(TableId table) => _databaseProvider switch
  {
    DatabaseProvider.PostgreSQL => PostgresDeleteBuilder.From(table),
    DatabaseProvider.SqlServer => SqlServerDeleteBuilder.From(table),
    _ => throw new NotSupportedException($"The database provider '{_databaseProvider}' is not supported."),
  };

  public Task DisposeAsync() => Task.CompletedTask;
}
