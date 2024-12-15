using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.Identity.EntityFrameworkCore.Relational.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Repositories;

[Trait(Traits.Category, Categories.Integration)]
public class OneTimePasswordRepositoryTests : OneTimePasswordRepositoryTestsBase, IAsyncLifetime
{
  public OneTimePasswordRepositoryTests() : base()
  {
  }

  protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    string connectionString = (configuration.GetValue<string>("POSTGRESQLCONNSTR_Identity") ?? string.Empty).Replace("{Database}", GetType().Name);
    services.AddLogitarIdentityWithEntityFrameworkCorePostgreSQL(connectionString);
  }

  protected override IDeleteBuilder CreateDeleteBuilder(TableId table) => PostgresDeleteBuilder.From(table);
}
