using Bogus;
using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

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

    ServiceCollection services = new();
    services.AddSingleton(configuration);
    ConfigureServices(services, configuration);
    ServiceProvider = services.BuildServiceProvider();

    EventContext = ServiceProvider.GetRequiredService<EventContext>();
    IdentityContext = ServiceProvider.GetRequiredService<IdentityContext>();
  }

  protected abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);

  protected abstract IDeleteBuilder CreateDeleteBuilder(TableId table);
}
