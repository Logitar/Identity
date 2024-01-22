using Bogus;
using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EFCore.SqlServer.IntegrationTests.Repositories;

[Trait(Traits.Category, Categories.Integration)]
public class OneTimePasswordRepositoryTests : IAsyncLifetime
{
  private const string PasswordString = "284199";

  private readonly Faker _faker = new();

  private readonly EventContext _eventContext;
  private readonly IdentityContext _identityContext;
  private readonly IOneTimePasswordRepository _oneTimePasswordRepository;
  private readonly IPasswordManager _passwordManager;
  private readonly IServiceProvider _serviceProvider;

  private readonly Password _password;
  private readonly OneTimePasswordAggregate _oneTimePassword;

  public OneTimePasswordRepositoryTests()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .Build();

    string connectionString = (configuration.GetValue<string>("SQLCONNSTR_Identity") ?? string.Empty)
      .Replace("{Database}", nameof(OneTimePasswordRepositoryTests));

    _serviceProvider = new ServiceCollection()
      .AddSingleton(configuration)
      .AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString)
      .BuildServiceProvider();

    _eventContext = _serviceProvider.GetRequiredService<EventContext>();
    _identityContext = _serviceProvider.GetRequiredService<IdentityContext>();
    _oneTimePasswordRepository = _serviceProvider.GetRequiredService<IOneTimePasswordRepository>();
    _passwordManager = _serviceProvider.GetRequiredService<IPasswordManager>();

    TenantId tenantId = new("tests");
    DateTime expiresOn = DateTime.Now.AddHours(1);
    int maximumAttempts = 5;
    ActorId actorId = ActorId.NewId();
    OneTimePasswordId id = OneTimePasswordId.NewId();

    _password = _passwordManager.Create(PasswordString, validate: false); // TODO(fpion): refactor
    _oneTimePassword = new(_password, tenantId, expiresOn, maximumAttempts, actorId, id);
  }

  public async Task InitializeAsync()
  {
    await _eventContext.Database.MigrateAsync();
    await _identityContext.Database.MigrateAsync();

    TableId[] tables = [IdentityDb.OneTimePasswords.Table, IdentityDb.CustomAttributes.Table, EventDb.Events.Table];
    foreach (TableId table in tables)
    {
      ICommand command = SqlServerDeleteBuilder.From(table).Build();
      await _identityContext.Database.ExecuteSqlRawAsync(command.Text, command.Parameters.ToArray());
    }

    await _oneTimePasswordRepository.SaveAsync(_oneTimePassword);
  }

  [Fact(DisplayName = "LoadAsync: it should load all the One-Time Passwords.")]
  public async Task LoadAsync_it_should_load_all_the_One_Time_Passwords()
  {
    OneTimePasswordAggregate deleted = new(_password, tenantId: null);
    deleted.Delete();
    await _oneTimePasswordRepository.SaveAsync(deleted);

    IEnumerable<OneTimePasswordAggregate> oneTimePasswords = await _oneTimePasswordRepository.LoadAsync(includeDeleted: true);
    Assert.Equal(2, oneTimePasswords.Count());
    Assert.Contains(oneTimePasswords, _oneTimePassword.Equals);
    Assert.Contains(oneTimePasswords, deleted.Equals);
  }

  [Fact(DisplayName = "LoadAsync: it should load the One-Time Password by identifier.")]
  public async Task LoadAsync_it_should_load_the_One_Time_Password_by_identifier()
  {
    Assert.Null(await _oneTimePasswordRepository.LoadAsync(OneTimePasswordId.NewId()));

    _oneTimePassword.Delete();
    long version = _oneTimePassword.Version;

    _oneTimePassword.Validate(PasswordString);
    await _oneTimePasswordRepository.SaveAsync(_oneTimePassword);

    Assert.Null(await _oneTimePasswordRepository.LoadAsync(_oneTimePassword.Id, version));

    OneTimePasswordAggregate? oneTimePassword = await _oneTimePasswordRepository.LoadAsync(_oneTimePassword.Id, version, includeDeleted: true);
    Assert.NotNull(oneTimePassword);
    Assert.Equal(_oneTimePassword, oneTimePassword);
  }

  [Fact(DisplayName = "LoadAsync: it should load the One-Time Passwords by tenant identifier.")]
  public async Task LoadAsync_it_should_load_the_One_Time_Passwords_by_tenant_identifier()
  {
    OneTimePasswordAggregate oneTimePassword = new(_password, tenantId: null);
    OneTimePasswordAggregate deleted = new(_password, _oneTimePassword.TenantId);
    deleted.Delete();
    await _oneTimePasswordRepository.SaveAsync([oneTimePassword, deleted]);

    IEnumerable<OneTimePasswordAggregate> oneTimePasswords = await _oneTimePasswordRepository.LoadAsync(_oneTimePassword.TenantId, includeDeleted: false);
    Assert.Equal(_oneTimePassword, oneTimePasswords.Single());
  }

  [Fact(DisplayName = "LoadAsync: it should load the One-Time Passwords by identifiers.")]
  public async Task LoadAsync_it_should_load_the_One_Time_Passwords_by_identifiers()
  {
    OneTimePasswordAggregate deleted = new(_password, tenantId: null);
    deleted.Delete();
    await _oneTimePasswordRepository.SaveAsync(deleted);

    IEnumerable<OneTimePasswordAggregate> oneTimePasswords = await _oneTimePasswordRepository.LoadAsync([_oneTimePassword.Id, deleted.Id, OneTimePasswordId.NewId()], includeDeleted: true);
    Assert.Equal(2, oneTimePasswords.Count());
    Assert.Contains(oneTimePasswords, _oneTimePassword.Equals);
    Assert.Contains(oneTimePasswords, deleted.Equals);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified One-Time Password.")]
  public async Task SaveAsync_it_should_save_the_specified_One_Time_Password()
  {
    OneTimePasswordEntity? entity = await _identityContext.OneTimePasswords.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _oneTimePassword.Id.Value);
    Assert.NotNull(entity);
    AssertOneTimePasswords.AreEqual(_oneTimePassword, entity);

    Dictionary<string, string> customAttributes = await _identityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.OneTimePasswords) && x.EntityId == entity.OneTimePasswordId)
      .ToDictionaryAsync(x => x.Key, x => x.Value);
    Assert.Equal(_oneTimePassword.CustomAttributes, customAttributes);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified One-Time Passwords.")]
  public async Task SaveAsync_it_should_save_the_specified_One_Time_Passwords()
  {
    OneTimePasswordAggregate succeeded = new(_password);
    OneTimePasswordAggregate deleted = new(_password);
    await _oneTimePasswordRepository.SaveAsync([succeeded, deleted]);

    Dictionary<string, OneTimePasswordEntity> entities = await _identityContext.OneTimePasswords.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(succeeded.Id.Value));
    Assert.True(entities.ContainsKey(deleted.Id.Value));

    succeeded.Validate(PasswordString);
    deleted.Delete();
    await _oneTimePasswordRepository.SaveAsync([succeeded, deleted]);

    entities = await _identityContext.OneTimePasswords.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(succeeded.Id.Value));
    Assert.False(entities.ContainsKey(deleted.Id.Value));

    AssertOneTimePasswords.AreEqual(succeeded, entities[succeeded.Id.Value]);
  }

  public Task DisposeAsync() => Task.CompletedTask;
}
