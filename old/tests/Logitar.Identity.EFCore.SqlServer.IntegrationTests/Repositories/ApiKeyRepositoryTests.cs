using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.Identity.EntityFrameworkCore.Relational.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

[Trait(Traits.Category, Categories.Integration)]
public class ApiKeyRepositoryTests : ApiKeyRepositoryTestsBase, IAsyncLifetime
{
  public ApiKeyRepositoryTests() : base()
  {
  }

  protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    string connectionString = (configuration.GetValue<string>("SQLCONNSTR_Identity") ?? string.Empty).Replace("{Database}", GetType().Name);
    services.AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString);
  }

  protected override IDeleteBuilder CreateDeleteBuilder(TableId table) => SqlServerDeleteBuilder.From(table);
}
