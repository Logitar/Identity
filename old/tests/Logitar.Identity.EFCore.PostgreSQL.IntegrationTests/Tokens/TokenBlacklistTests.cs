using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.Identity.EntityFrameworkCore.Relational.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Tokens;

[Trait(Traits.Category, Categories.Integration)]
public class TokenBlacklistTests : TokenBlacklistTestsBase, IAsyncLifetime
{
  public TokenBlacklistTests() : base()
  {
  }

  protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    string connectionString = (configuration.GetValue<string>("POSTGRESQLCONNSTR_Identity") ?? string.Empty).Replace("{Database}", GetType().Name);
    services.AddLogitarIdentityWithEntityFrameworkCorePostgreSQL(connectionString);
  }

  protected override IDeleteBuilder CreateDeleteBuilder(TableId table) => PostgresDeleteBuilder.From(table);
}
