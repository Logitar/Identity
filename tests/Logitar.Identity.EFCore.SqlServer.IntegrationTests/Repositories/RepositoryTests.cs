using Bogus;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

public abstract class RepositoryTests
{
  protected Faker Faker { get; } = new();

  protected IServiceProvider ServiceProvider { get; }
  protected EventContext EventContext { get; }
  protected IdentityContext IdentityContext { get; }

  protected RepositoryTests()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .Build();

    string connectionString = (configuration.GetValue<string>("SQLCONNSTR_Identity") ?? string.Empty)
      .Replace("{Database}", GetType().Name);

    ServiceProvider = new ServiceCollection()
      .AddSingleton(configuration)
      .AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString)
      .BuildServiceProvider();

    EventContext = ServiceProvider.GetRequiredService<EventContext>();
    IdentityContext = ServiceProvider.GetRequiredService<IdentityContext>();
  }
}
