using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

[Trait(Traits.Category, Categories.Integration)]
public class OneTimePasswordRepositoryTests : RepositoryTests, IAsyncLifetime
{
  private const string PasswordString = "284199";

  private readonly IOneTimePasswordRepository _oneTimePasswordRepository;
  private readonly IPasswordManager _passwordManager;

  private readonly Password _password;
  private readonly OneTimePasswordAggregate _oneTimePassword;

  public OneTimePasswordRepositoryTests() : base()
  {
    _oneTimePasswordRepository = ServiceProvider.GetRequiredService<IOneTimePasswordRepository>();
    _passwordManager = ServiceProvider.GetRequiredService<IPasswordManager>();

    TenantId tenantId = new("tests");
    DateTime expiresOn = DateTime.Now.AddHours(1);
    int maximumAttempts = 5;
    ActorId actorId = ActorId.NewId();
    OneTimePasswordId id = OneTimePasswordId.NewId();

    _password = _passwordManager.Create(PasswordString);
    _oneTimePassword = new(_password, tenantId, expiresOn, maximumAttempts, actorId, id);
    _oneTimePassword.SetCustomAttribute("Purpose", "MultiFactorAuthentication");
    _oneTimePassword.SetCustomAttribute("UserId", Guid.NewGuid().ToString());
    _oneTimePassword.Update(actorId);
  }

  public async Task InitializeAsync()
  {
    await EventContext.Database.MigrateAsync();
    await IdentityContext.Database.MigrateAsync();

    TableId[] tables = [IdentityDb.OneTimePasswords.Table, IdentityDb.CustomAttributes.Table, EventDb.Events.Table];
    foreach (TableId table in tables)
    {
      ICommand command = SqlServerDeleteBuilder.From(table).Build();
      await IdentityContext.Database.ExecuteSqlRawAsync(command.Text, command.Parameters.ToArray());
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

    IEnumerable<OneTimePasswordAggregate> oneTimePasswords = await _oneTimePasswordRepository.LoadAsync(_oneTimePassword.TenantId);
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

  [Fact(DisplayName = "SaveAsync: it should save the deleted One-Time Password.")]
  public async Task SaveAsync_it_should_save_the_deleted_One_Time_Password()
  {
    _oneTimePassword.SetCustomAttribute("Purpose", "reset_password");
    _oneTimePassword.SetCustomAttribute("UserId", UserId.NewId().Value);
    _oneTimePassword.Update();
    await _oneTimePasswordRepository.SaveAsync(_oneTimePassword);

    OneTimePasswordEntity? entity = await IdentityContext.OneTimePasswords.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _oneTimePassword.Id.Value);
    Assert.NotNull(entity);

    CustomAttributeEntity[] customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.OneTimePasswords) && x.EntityId == entity.OneTimePasswordId)
      .ToArrayAsync();
    Assert.Equal(_oneTimePassword.CustomAttributes.Count, customAttributes.Length);
    foreach (KeyValuePair<string, string> customAttribute in _oneTimePassword.CustomAttributes)
    {
      Assert.Contains(customAttributes, c => c.Key == customAttribute.Key && c.Value == customAttribute.Value);
    }

    _oneTimePassword.Delete();
    await _oneTimePasswordRepository.SaveAsync(_oneTimePassword);

    customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.OneTimePasswords) && x.EntityId == entity.OneTimePasswordId)
      .ToArrayAsync();
    Assert.Empty(customAttributes);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified One-Time Password.")]
  public async Task SaveAsync_it_should_save_the_specified_One_Time_Password()
  {
    OneTimePasswordEntity? entity = await IdentityContext.OneTimePasswords.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _oneTimePassword.Id.Value);
    Assert.NotNull(entity);
    AssertOneTimePasswords.AreEqual(_oneTimePassword, entity);

    Dictionary<string, string> customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.OneTimePasswords) && x.EntityId == entity.OneTimePasswordId)
      .ToDictionaryAsync(x => x.Key, x => x.Value);
    Assert.Equal(_oneTimePassword.CustomAttributes, customAttributes);

    OneTimePasswordAggregate? oneTimePassword = await _oneTimePasswordRepository.LoadAsync(_oneTimePassword.Id);
    Assert.NotNull(oneTimePassword);
    Assert.Equal(_oneTimePassword.CustomAttributes, oneTimePassword.CustomAttributes);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified One-Time Passwords.")]
  public async Task SaveAsync_it_should_save_the_specified_One_Time_Passwords()
  {
    OneTimePasswordAggregate succeeded = new(_password);
    OneTimePasswordAggregate deleted = new(_password);
    await _oneTimePasswordRepository.SaveAsync([succeeded, deleted]);

    Dictionary<string, OneTimePasswordEntity> entities = await IdentityContext.OneTimePasswords.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(succeeded.Id.Value));
    Assert.True(entities.ContainsKey(deleted.Id.Value));

    succeeded.Validate(PasswordString);
    deleted.Delete();
    await _oneTimePasswordRepository.SaveAsync([succeeded, deleted]);

    entities = await IdentityContext.OneTimePasswords.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(succeeded.Id.Value));
    Assert.False(entities.ContainsKey(deleted.Id.Value));

    AssertOneTimePasswords.AreEqual(succeeded, entities[succeeded.Id.Value]);
  }

  public Task DisposeAsync() => Task.CompletedTask;
}
