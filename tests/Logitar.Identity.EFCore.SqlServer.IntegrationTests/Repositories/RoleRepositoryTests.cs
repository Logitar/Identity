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
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

[Trait(Traits.Category, Categories.Integration)]
public class RoleRepositoryTests : RepositoryTests, IAsyncLifetime
{
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IPasswordManager _passwordManager;
  private readonly IRoleRepository _roleRepository;
  private readonly IRoleSettings _roleSettings;
  private readonly IUserRepository _userRepository;
  private readonly IUserSettings _userSettings;

  private readonly RoleAggregate _role;

  public RoleRepositoryTests() : base()
  {
    _apiKeyRepository = ServiceProvider.GetRequiredService<IApiKeyRepository>();
    _passwordManager = ServiceProvider.GetRequiredService<IPasswordManager>();
    _roleRepository = ServiceProvider.GetRequiredService<IRoleRepository>();
    _roleSettings = ServiceProvider.GetRequiredService<IRoleSettingsResolver>().Resolve();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    _userSettings = ServiceProvider.GetRequiredService<IUserSettingsResolver>().Resolve();

    RoleId roleId = RoleId.NewId();
    ActorId actorId = new(roleId.Value);
    TenantId tenantId = new("tests");

    UniqueNameUnit uniqueName = new(_roleSettings.UniqueName, "send_messages");
    _role = new(uniqueName, tenantId, actorId, roleId)
    {
      DisplayName = new DisplayNameUnit("Send Messages"),
      Description = new DescriptionUnit("This role allows to send messages.")
    };
    _role.SetCustomAttribute("manage_messages", bool.TrueString);
    _role.Update(actorId);
  }

  public async Task InitializeAsync()
  {
    await EventContext.Database.MigrateAsync();
    await IdentityContext.Database.MigrateAsync();

    TableId[] tables =
    [
      IdentityDb.Roles.Table,
      IdentityDb.CustomAttributes.Table,
      EventDb.Events.Table
    ];
    foreach (TableId table in tables)
    {
      ICommand command = SqlServerDeleteBuilder.From(table).Build();
      await IdentityContext.Database.ExecuteSqlRawAsync(command.Text, command.Parameters.ToArray());
    }

    await _roleRepository.SaveAsync(_role);
    await _roleRepository.SaveAsync(_role);
  }

  [Fact(DisplayName = "LoadAsync: it should load all the roles.")]
  public async Task LoadAsync_it_should_load_all_the_roles()
  {
    RoleAggregate deleted = new(_role.UniqueName, tenantId: null);
    deleted.Delete();
    await _roleRepository.SaveAsync(deleted);

    IEnumerable<RoleAggregate> roles = await _roleRepository.LoadAsync(includeDeleted: true);
    Assert.Equal(2, roles.Count());
    Assert.Contains(roles, _role.Equals);
    Assert.Contains(roles, deleted.Equals);
  }

  [Fact(DisplayName = "LoadAsync: it should load the role by identifier.")]
  public async Task LoadAsync_it_should_load_the_role_by_identifier()
  {
    Assert.Null(await _roleRepository.LoadAsync(RoleId.NewId()));

    _role.Delete();
    long version = _role.Version;

    UniqueNameUnit oldUniqueName = _role.UniqueName;
    _role.SetUniqueName(new UniqueNameUnit(_roleSettings.UniqueName, "guest"));
    await _roleRepository.SaveAsync(_role);

    Assert.Null(await _roleRepository.LoadAsync(_role.Id, version));

    RoleAggregate? role = await _roleRepository.LoadAsync(_role.Id, version, includeDeleted: true);
    Assert.NotNull(role);
    Assert.Equal(oldUniqueName, role.UniqueName);
    Assert.Equal(_role, role);
  }

  [Fact(DisplayName = "LoadAsync: it should load the roles by tenant identifier.")]
  public async Task LoadAsync_it_should_load_the_roles_by_tenant_identifier()
  {
    RoleAggregate role = new(_role.UniqueName, tenantId: null);
    RoleAggregate deleted = new(new UniqueNameUnit(_roleSettings.UniqueName, "deleted"), _role.TenantId);
    deleted.Delete();
    await _roleRepository.SaveAsync([role, deleted]);

    IEnumerable<RoleAggregate> roles = await _roleRepository.LoadAsync(_role.TenantId);
    Assert.Equal(_role, roles.Single());
  }

  [Fact(DisplayName = "LoadAsync: it should load the role by unique name.")]
  public async Task LoadAsync_it_should_load_the_role_by_unique_name()
  {
    Assert.Null(await _roleRepository.LoadAsync(tenantId: null, _role.UniqueName));

    UniqueNameUnit uniqueName = new(_roleSettings.UniqueName, $"{_role.UniqueName.Value}2");
    Assert.Null(await _roleRepository.LoadAsync(_role.TenantId, uniqueName));

    RoleAggregate? role = await _roleRepository.LoadAsync(_role.TenantId, _role.UniqueName);
    Assert.NotNull(role);
    Assert.Equal(_role, role);
  }

  [Fact(DisplayName = "LoadAsync: it should load the roles by identifiers.")]
  public async Task LoadAsync_it_should_load_the_roles_by_identifiers()
  {
    RoleAggregate deleted = new(_role.UniqueName, tenantId: null);
    deleted.Delete();
    await _roleRepository.SaveAsync(deleted);

    IEnumerable<RoleAggregate> roles = await _roleRepository.LoadAsync([_role.Id, deleted.Id, RoleId.NewId()], includeDeleted: true);
    Assert.Equal(2, roles.Count());
    Assert.Contains(roles, _role.Equals);
    Assert.Contains(roles, deleted.Equals);
  }

  [Fact(DisplayName = "LoadAsync: it should load the roles by API key.")]
  public async Task LoadAsync_it_should_load_the_roles_by_Api_key()
  {
    RoleAggregate manageUsers = new(new UniqueNameUnit(_roleSettings.UniqueName, "manage_users"));
    await _roleRepository.SaveAsync(manageUsers);

    Password secret = _passwordManager.GenerateBase64(32, out _);
    ApiKeyAggregate apiKey = new(new DisplayNameUnit("Default"), secret);
    apiKey.AddRole(manageUsers);
    await _apiKeyRepository.SaveAsync(apiKey);

    RoleAggregate role = Assert.Single(await _roleRepository.LoadAsync(apiKey));
    Assert.Equal(manageUsers, role);
  }

  [Fact(DisplayName = "LoadAsync: it should load the roles by user.")]
  public async Task LoadAsync_it_should_load_the_roles_by_user()
  {
    RoleAggregate manageUsers = new(new UniqueNameUnit(_roleSettings.UniqueName, "manage_users"));
    await _roleRepository.SaveAsync(manageUsers);

    UserAggregate user = new(new UniqueNameUnit(_userSettings.UniqueName, Faker.Internet.UserName()));
    user.AddRole(manageUsers);
    await _userRepository.SaveAsync(user);

    RoleAggregate role = Assert.Single(await _roleRepository.LoadAsync(user));
    Assert.Equal(manageUsers, role);
  }

  [Fact(DisplayName = "SaveAsync: it should save the deleted role.")]
  public async Task SaveAsync_it_should_save_the_deleted_role()
  {
    _role.SetCustomAttribute("read_messages", bool.TrueString);
    _role.SetCustomAttribute("edit_messages", bool.FalseString);
    _role.Update();
    await _roleRepository.SaveAsync(_role);

    RoleEntity? entity = await IdentityContext.Roles.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _role.Id.Value);
    Assert.NotNull(entity);

    CustomAttributeEntity[] customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.Roles) && x.EntityId == entity.RoleId)
      .ToArrayAsync();
    Assert.Equal(_role.CustomAttributes.Count, customAttributes.Length);
    foreach (KeyValuePair<string, string> customAttribute in _role.CustomAttributes)
    {
      Assert.Contains(customAttributes, c => c.Key == customAttribute.Key && c.Value == customAttribute.Value);
    }

    _role.Delete();
    await _roleRepository.SaveAsync(_role);

    customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.Roles) && x.EntityId == entity.RoleId)
      .ToArrayAsync();
    Assert.Empty(customAttributes);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified role.")]
  public async Task SaveAsync_it_should_save_the_specified_role()
  {
    RoleEntity? entity = await IdentityContext.Roles.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _role.Id.Value);
    Assert.NotNull(entity);
    AssertRoles.AreEqual(_role, entity);

    Dictionary<string, string> customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.Roles) && x.EntityId == entity.RoleId)
      .ToDictionaryAsync(x => x.Key, x => x.Value);
    Assert.Equal(_role.CustomAttributes, customAttributes);

    RoleAggregate? role = await _roleRepository.LoadAsync(_role.Id);
    Assert.NotNull(role);
    Assert.Equal(_role.DisplayName, role.DisplayName);
    Assert.Equal(_role.Description, role.Description);
    Assert.Equal(_role.CustomAttributes, role.CustomAttributes);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified roles.")]
  public async Task SaveAsync_it_should_save_the_specified_roles()
  {
    RoleAggregate guest = new(new UniqueNameUnit(_roleSettings.UniqueName, "gwest"));
    RoleAggregate deleted = new(new UniqueNameUnit(_roleSettings.UniqueName, "deleted"));
    await _roleRepository.SaveAsync([guest, deleted]);

    Dictionary<string, RoleEntity> entities = await IdentityContext.Roles.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(guest.Id.Value));
    Assert.True(entities.ContainsKey(deleted.Id.Value));

    guest.SetUniqueName(new UniqueNameUnit(_roleSettings.UniqueName, "guest"));
    deleted.Delete();
    await _roleRepository.SaveAsync([guest, deleted]);

    entities = await IdentityContext.Roles.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(guest.Id.Value));
    Assert.False(entities.ContainsKey(deleted.Id.Value));

    AssertRoles.AreEqual(guest, entities[guest.Id.Value]);
  }

  public Task DisposeAsync() => Task.CompletedTask;
}
