using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

[Trait(Traits.Category, Categories.Integration)]
public class ApiKeyRepositoryTests : RepositoryTests, IAsyncLifetime
{
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IPasswordManager _passwordManager;
  private readonly IRoleRepository _roleRepository;
  private readonly IRoleSettings _roleSettings;

  private readonly Password _secret;
  private readonly string _secretString;

  private readonly ApiKeyAggregate _apiKey;
  private readonly RoleAggregate _role;

  public ApiKeyRepositoryTests() : base()
  {
    _apiKeyRepository = ServiceProvider.GetRequiredService<IApiKeyRepository>();
    _passwordManager = ServiceProvider.GetRequiredService<IPasswordManager>();
    _roleRepository = ServiceProvider.GetRequiredService<IRoleRepository>();
    _roleSettings = ServiceProvider.GetRequiredService<IRoleSettingsResolver>().Resolve();

    ApiKeyId apiKeyId = ApiKeyId.NewId();
    ActorId actorId = new(apiKeyId.Value);
    TenantId tenantId = new("tests");

    _role = new(new UniqueNameUnit(_roleSettings.UniqueName, "clerk"), tenantId, actorId);

    DisplayNameUnit displayName = new("Default");
    _secret = _passwordManager.GenerateBase64(32, out _secretString);
    _apiKey = new(displayName, _secret, tenantId, actorId, apiKeyId)
    {
      Description = new DescriptionUnit("This is the default API key.")
    };
    _apiKey.SetExpiration(DateTime.Now.AddYears(1));
    _apiKey.SetCustomAttribute("TODO", "TODO");
    _apiKey.Update(actorId);

    _apiKey.AddRole(_role, actorId);

    _apiKey.Authenticate(_secretString, actorId);
  }

  public async Task InitializeAsync()
  {
    await EventContext.Database.MigrateAsync();
    await IdentityContext.Database.MigrateAsync();

    TableId[] tables =
    [
      IdentityDb.ApiKeys.Table,
      IdentityDb.Roles.Table,
      IdentityDb.CustomAttributes.Table,
      IdentityDb.Actors.Table,
      EventDb.Events.Table
    ];
    foreach (TableId table in tables)
    {
      ICommand command = SqlServerDeleteBuilder.From(table).Build();
      await IdentityContext.Database.ExecuteSqlRawAsync(command.Text, command.Parameters.ToArray());
    }

    await _roleRepository.SaveAsync(_role);
    await _apiKeyRepository.SaveAsync(_apiKey);
  }

  [Fact(DisplayName = "LoadAsync: it should load all the API keys.")]
  public async Task LoadAsync_it_should_load_all_the_Api_keys()
  {
    ApiKeyAggregate deleted = new(_apiKey.DisplayName, _secret, tenantId: null);
    deleted.Delete();
    await _apiKeyRepository.SaveAsync(deleted);

    IEnumerable<ApiKeyAggregate> apiKeys = await _apiKeyRepository.LoadAsync(includeDeleted: true);
    Assert.Equal(2, apiKeys.Count());
    Assert.Contains(apiKeys, _apiKey.Equals);
    Assert.Contains(apiKeys, deleted.Equals);
  }

  [Fact(DisplayName = "LoadAsync: it should load the API key by identifier.")]
  public async Task LoadAsync_it_should_load_the_Api_key_by_identifier()
  {
    Assert.Null(await _apiKeyRepository.LoadAsync(ApiKeyId.NewId()));

    _apiKey.Delete();
    long version = _apiKey.Version;

    DateTime? authenticatedOn = _apiKey.AuthenticatedOn;
    _apiKey.Authenticate(_secretString);
    await _apiKeyRepository.SaveAsync(_apiKey);

    Assert.Null(await _apiKeyRepository.LoadAsync(_apiKey.Id, version));

    ApiKeyAggregate? apiKey = await _apiKeyRepository.LoadAsync(_apiKey.Id, version, includeDeleted: true);
    Assert.NotNull(apiKey);
    Assert.Equal(authenticatedOn, apiKey.AuthenticatedOn);
    Assert.Equal(_apiKey, apiKey);
  }

  [Fact(DisplayName = "LoadAsync: it should load the API key by role.")]
  public async Task LoadAsync_it_should_load_the_Api_key_by_role()
  {
    RoleAggregate admin = new(new UniqueNameUnit(_roleSettings.UniqueName, "admin"), _apiKey.TenantId);
    await _roleRepository.SaveAsync(admin);

    Assert.Empty(await _apiKeyRepository.LoadAsync(admin));

    IEnumerable<ApiKeyAggregate> apiKeys = await _apiKeyRepository.LoadAsync(_role);
    Assert.Equal(_apiKey, apiKeys.Single());
  }

  [Fact(DisplayName = "LoadAsync: it should load the API keys by tenant identifier.")]
  public async Task LoadAsync_it_should_load_the_apiKeys_by_tenant_identifier()
  {
    ApiKeyAggregate apiKey = new(new DisplayNameUnit("Other API key"), _secret, tenantId: null);
    ApiKeyAggregate deleted = new(new DisplayNameUnit("deleted"), _secret, _apiKey.TenantId);
    deleted.Delete();
    await _apiKeyRepository.SaveAsync([apiKey, deleted]);

    IEnumerable<ApiKeyAggregate> apiKeys = await _apiKeyRepository.LoadAsync(_apiKey.TenantId);
    Assert.Equal(_apiKey, apiKeys.Single());
  }

  [Fact(DisplayName = "LoadAsync: it should load the API keys by identifiers.")]
  public async Task LoadAsync_it_should_load_the_Api_keys_by_identifiers()
  {
    ApiKeyAggregate deleted = new(new DisplayNameUnit("deleted"), _secret, tenantId: null);
    deleted.Delete();
    await _apiKeyRepository.SaveAsync(deleted);

    IEnumerable<ApiKeyAggregate> apiKeys = await _apiKeyRepository.LoadAsync([_apiKey.Id, deleted.Id, ApiKeyId.NewId()], includeDeleted: true);
    Assert.Equal(2, apiKeys.Count());
    Assert.Contains(apiKeys, _apiKey.Equals);
    Assert.Contains(apiKeys, deleted.Equals);
  }

  [Fact(DisplayName = "SaveAsync: it should save the deleted API key.")]
  public async Task SaveAsync_it_should_save_the_deleted_Api_key()
  {
    _apiKey.SetCustomAttribute("TODO", "TODO");
    _apiKey.SetCustomAttribute("TODO", "TODO");
    _apiKey.Update();
    await _apiKeyRepository.SaveAsync(_apiKey);

    ApiKeyEntity? entity = await IdentityContext.ApiKeys.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _apiKey.Id.Value);
    Assert.NotNull(entity);

    CustomAttributeEntity[] customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.ApiKeys) && x.EntityId == entity.ApiKeyId)
      .ToArrayAsync();
    Assert.Equal(_apiKey.CustomAttributes.Count, customAttributes.Length);
    foreach (KeyValuePair<string, string> customAttribute in _apiKey.CustomAttributes)
    {
      Assert.Contains(customAttributes, c => c.Key == customAttribute.Key && c.Value == customAttribute.Value);
    }

    _apiKey.Delete();
    await _apiKeyRepository.SaveAsync(_apiKey);

    customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.ApiKeys) && x.EntityId == entity.ApiKeyId)
      .ToArrayAsync();
    Assert.Empty(customAttributes);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified API key.")]
  public async Task SaveAsync_it_should_save_the_specified_Api_key()
  {
    ApiKeyEntity? entity = await IdentityContext.ApiKeys.AsNoTracking()
      .Include(x => x.Roles)
      .SingleOrDefaultAsync(x => x.AggregateId == _apiKey.Id.Value);
    Assert.NotNull(entity);
    AssertApiKeys.AreEqual(_apiKey, entity);

    ActorEntity? actor = await IdentityContext.Actors.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == _apiKey.Id.Value);
    AssertApiKeys.AreEquivalent(entity, actor);

    Dictionary<string, string> customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.ApiKeys) && x.EntityId == entity.ApiKeyId)
      .ToDictionaryAsync(x => x.Key, x => x.Value);
    Assert.Equal(_apiKey.CustomAttributes, customAttributes);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified API keys.")]
  public async Task SaveAsync_it_should_save_the_specified_Api_keys()
  {
    ApiKeyAggregate authenticated = new(new DisplayNameUnit("authenticated"), _secret);
    ApiKeyAggregate deleted = new(new DisplayNameUnit("deleted"), _secret);
    await _apiKeyRepository.SaveAsync([authenticated, deleted]);

    Dictionary<string, ApiKeyEntity> entities = await IdentityContext.ApiKeys.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(authenticated.Id.Value));
    Assert.True(entities.ContainsKey(deleted.Id.Value));

    authenticated.Authenticate(_secretString);
    deleted.Delete();
    await _apiKeyRepository.SaveAsync([authenticated, deleted]);

    entities = await IdentityContext.ApiKeys.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(authenticated.Id.Value));
    Assert.False(entities.ContainsKey(deleted.Id.Value));

    AssertApiKeys.AreEqual(authenticated, entities[authenticated.Id.Value]);

    ActorEntity? actor = await IdentityContext.Actors.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == deleted.Id.Value);
    Assert.NotNull(actor);
    Assert.True(actor.IsDeleted);
  }

  public Task DisposeAsync() => Task.CompletedTask;
}
