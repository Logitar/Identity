using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.Identity.EntityFrameworkCore.Relational.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Repositories;

[Trait(Traits.Category, Categories.Integration)]
public class SessionRepositoryTests : SessionRepositoryTestsBase, IAsyncLifetime
{
  public SessionRepositoryTests() : base()
  {
  }

  protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    string connectionString = (configuration.GetValue<string>("POSTGRESQLCONNSTR_Identity") ?? string.Empty).Replace("{Database}", GetType().Name);
    services.AddLogitarIdentityWithEntityFrameworkCorePostgreSQL(connectionString);
  }

  protected override IDeleteBuilder CreateDeleteBuilder(TableId table) => PostgresDeleteBuilder.From(table);
}
