using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public abstract class UserRepositoryTestsBase : RepositoryTests, IAsyncLifetime
{
  private const string EmployeeIdKey = "EmployeeId";
  private const string PasswordString = "P@s$W0rD";

  private readonly IPasswordManager _passwordManager;
  private readonly IRoleRepository _roleRepository;
  private readonly IRoleSettings _roleSettings;
  private readonly IUserRepository _userRepository;
  private readonly IUserSettings _userSettings;

  private readonly RoleAggregate _role;
  private readonly UserAggregate _user;

  public UserRepositoryTestsBase() : base()
  {
    _passwordManager = ServiceProvider.GetRequiredService<IPasswordManager>();
    _roleRepository = ServiceProvider.GetRequiredService<IRoleRepository>();
    _roleSettings = ServiceProvider.GetRequiredService<IRoleSettingsResolver>().Resolve();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    _userSettings = ServiceProvider.GetRequiredService<IUserSettingsResolver>().Resolve();

    UserId userId = UserId.NewId();
    ActorId actorId = new(userId.Value);
    TenantId tenantId = new("tests");

    _role = new(new UniqueNameUnit(_roleSettings.UniqueName, "clerk"), tenantId, actorId);

    UniqueNameUnit uniqueName = new(_userSettings.UniqueName, Faker.Person.UserName);
    _user = new(uniqueName, tenantId, actorId, userId)
    {
      FirstName = new PersonNameUnit(Faker.Person.FirstName),
      MiddleName = new PersonNameUnit(Faker.Name.FirstName()),
      LastName = new PersonNameUnit(Faker.Person.LastName),
      Nickname = new PersonNameUnit("Toto"),
      Birthdate = Faker.Person.DateOfBirth,
      Gender = new GenderUnit(Faker.Person.Gender.ToString()),
      Locale = new LocaleUnit(Faker.Locale),
      TimeZone = new TimeZoneUnit("America/Montreal"),
      Picture = new UrlUnit($"https://www.{Faker.Person.Website}/img/profile.jpg"),
      Profile = new UrlUnit($"https://www.desjardins.com/profiles/toto"),
      Website = new UrlUnit($"https://www.{Faker.Person.Website}")
    };
    _user.SetCustomAttribute("Department", "Finances");
    _user.Update(actorId);

    _user.SetAddress(new AddressUnit("150 Saint-Catherine St W", "Montreal", "CA", "QC", "H2X 3Y2"), actorId);
    _user.SetEmail(new EmailUnit(Faker.Person.Email, isVerified: true), actorId);
    _user.SetPhone(new PhoneUnit("+1 (514) 845-4636", "CA", "123456"), actorId);

    _user.AddRole(_role, actorId);
    _user.SetCustomIdentifier(EmployeeIdKey, Guid.NewGuid().ToString(), actorId);
    _user.SetPassword(_passwordManager.Create(PasswordString), actorId);

    _user.Authenticate(PasswordString, actorId);
  }

  public async Task InitializeAsync()
  {
    await EventContext.Database.MigrateAsync();
    await IdentityContext.Database.MigrateAsync();

    TableId[] tables =
    [
      IdentityDb.Sessions.Table,
      IdentityDb.Users.Table,
      IdentityDb.ApiKeys.Table,
      IdentityDb.Roles.Table,
      IdentityDb.CustomAttributes.Table,
      IdentityDb.Actors.Table,
      EventDb.Events.Table
    ];
    foreach (TableId table in tables)
    {
      ICommand command = CreateDeleteBuilder(table).Build();
      await IdentityContext.Database.ExecuteSqlRawAsync(command.Text, command.Parameters.ToArray());
    }

    await _roleRepository.SaveAsync(_role);
    await _userRepository.SaveAsync(_user);
  }

  [Fact(DisplayName = "LoadAsync: it should load all the users.")]
  public async Task LoadAsync_it_should_load_all_the_users()
  {
    UserAggregate deleted = new(_user.UniqueName, tenantId: null);
    deleted.Delete();
    await _userRepository.SaveAsync(deleted);

    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(includeDeleted: true);
    Assert.Equal(2, users.Count());
    Assert.Contains(users, _user.Equals);
    Assert.Contains(users, deleted.Equals);
  }

  [Fact(DisplayName = "LoadAsync: it should load the user by custom identifier.")]
  public async Task LoadAsync_it_should_load_the_user_by_custom_identifier()
  {
    Assert.Null(await _userRepository.LoadAsync(tenantId: null, EmployeeIdKey, _user.CustomIdentifiers[EmployeeIdKey]));
    Assert.Null(await _userRepository.LoadAsync(_user.TenantId, "EmployeeNo", _user.CustomIdentifiers[EmployeeIdKey]));
    Assert.Null(await _userRepository.LoadAsync(_user.TenantId, EmployeeIdKey, Guid.NewGuid().ToString()));

    UserAggregate? user = await _userRepository.LoadAsync(_user.TenantId, EmployeeIdKey, _user.CustomIdentifiers[EmployeeIdKey]);
    Assert.NotNull(user);
    Assert.Equal(_user, user);
  }

  [Fact(DisplayName = "LoadAsync: it should load the user by identifier.")]
  public async Task LoadAsync_it_should_load_the_user_by_identifier()
  {
    Assert.Null(await _userRepository.LoadAsync(UserId.NewId()));

    _user.Delete();
    long version = _user.Version;

    _user.Disable();
    await _userRepository.SaveAsync(_user);

    Assert.Null(await _userRepository.LoadAsync(_user.Id, version));

    UserAggregate? user = await _userRepository.LoadAsync(_user.Id, version, includeDeleted: true);
    Assert.NotNull(user);
    Assert.False(user.IsDisabled);
    Assert.Equal(_user, user);
  }

  [Fact(DisplayName = "LoadAsync: it should load the user by role.")]
  public async Task LoadAsync_it_should_load_the_user_by_role()
  {
    RoleAggregate admin = new(new UniqueNameUnit(_roleSettings.UniqueName, "admin"), _user.TenantId);
    await _roleRepository.SaveAsync(admin);

    Assert.Empty(await _userRepository.LoadAsync(admin));

    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(_role);
    Assert.Equal(_user, users.Single());
  }

  [Fact(DisplayName = "LoadAsync: it should load the user by session.")]
  public async Task LoadAsync_it_should_load_the_user_by_session()
  {
    SessionAggregate session = _user.SignIn();

    UserAggregate? user = await _userRepository.LoadAsync(session);
    Assert.NotNull(user);
    Assert.Equal(_user, user);
  }

  [Fact(DisplayName = "LoadAsync: it should load the users by tenant identifier.")]
  public async Task LoadAsync_it_should_load_the_users_by_tenant_identifier()
  {
    UserAggregate user = new(_user.UniqueName, tenantId: null);
    UserAggregate deleted = new(new UniqueNameUnit(_userSettings.UniqueName, "deleted"), _user.TenantId);
    deleted.Delete();
    await _userRepository.SaveAsync([user, deleted]);

    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(_user.TenantId);
    Assert.Equal(_user, users.Single());
  }

  [Fact(DisplayName = "LoadAsync: it should load the user by unique name.")]
  public async Task LoadAsync_it_should_load_the_user_by_unique_name()
  {
    Assert.Null(await _userRepository.LoadAsync(tenantId: null, _user.UniqueName));

    UniqueNameUnit uniqueName = new(_userSettings.UniqueName, $"{_user.UniqueName.Value}2");
    Assert.Null(await _userRepository.LoadAsync(_user.TenantId, uniqueName));

    UserAggregate? user = await _userRepository.LoadAsync(_user.TenantId, _user.UniqueName);
    Assert.NotNull(user);
    Assert.Equal(_user, user);
  }

  [Fact(DisplayName = "LoadAsync: it should load the users by email address.")]
  public async Task LoadAsync_it_should_load_the_users_by_email_address()
  {
    Assert.NotNull(_user.Email);

    Assert.Empty(await _userRepository.LoadAsync(tenantId: null, _user.Email));

    EmailUnit email = new($"{_user.Email.Address}2");
    Assert.Empty(await _userRepository.LoadAsync(_user.TenantId, email));

    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(_user.TenantId, _user.Email);
    Assert.Equal(_user, users.Single());

    UserAggregate other = new(new UniqueNameUnit(_userSettings.UniqueName, $"{_user.UniqueName.Value}2"), _user.TenantId);
    other.SetEmail(_user.Email);
    await _userRepository.SaveAsync(other);

    users = await _userRepository.LoadAsync(_user.TenantId, _user.Email);
    Assert.Equal(2, users.Count());
    Assert.Contains(users, _user.Equals);
    Assert.Contains(users, other.Equals);
  }

  [Fact(DisplayName = "LoadAsync: it should load the users by identifiers.")]
  public async Task LoadAsync_it_should_load_the_users_by_identifiers()
  {
    UserAggregate deleted = new(_user.UniqueName, tenantId: null);
    deleted.Delete();
    await _userRepository.SaveAsync(deleted);

    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync([_user.Id, deleted.Id, UserId.NewId()], includeDeleted: true);
    Assert.Equal(2, users.Count());
    Assert.Contains(users, _user.Equals);
    Assert.Contains(users, deleted.Equals);
  }

  [Fact(DisplayName = "SaveAsync: it should save the deleted user.")]
  public async Task SaveAsync_it_should_save_the_deleted_user()
  {
    _user.SetCustomAttribute("HealthInsuranceNumber", Faker.Person.BuildHealthInsuranceNumber());
    _user.SetCustomAttribute("JobTitle", "Sales Manager");
    _user.Update();
    await _userRepository.SaveAsync(_user);

    UserEntity? entity = await IdentityContext.Users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _user.Id.Value);
    Assert.NotNull(entity);

    CustomAttributeEntity[] customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.Users) && x.EntityId == entity.UserId)
      .ToArrayAsync();
    Assert.Equal(_user.CustomAttributes.Count, customAttributes.Length);
    foreach (KeyValuePair<string, string> customAttribute in _user.CustomAttributes)
    {
      Assert.Contains(customAttributes, c => c.Key == customAttribute.Key && c.Value == customAttribute.Value);
    }

    _user.Delete();
    await _userRepository.SaveAsync(_user);

    customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.Users) && x.EntityId == entity.UserId)
      .ToArrayAsync();
    Assert.Empty(customAttributes);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified user.")]
  public async Task SaveAsync_it_should_save_the_specified_user()
  {
    UserEntity? entity = await IdentityContext.Users.AsNoTracking()
      .Include(x => x.Identifiers)
      .Include(x => x.Roles)
      .SingleOrDefaultAsync(x => x.AggregateId == _user.Id.Value);
    Assert.NotNull(entity);
    AssertUsers.AreEqual(_user, entity);

    ActorEntity? actor = await IdentityContext.Actors.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == _user.Id.Value);
    AssertUsers.AreEquivalent(entity, actor);

    Dictionary<string, string> customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.Users) && x.EntityId == entity.UserId)
      .ToDictionaryAsync(x => x.Key, x => x.Value);
    Assert.Equal(_user.CustomAttributes, customAttributes);

    UserAggregate? user = await _userRepository.LoadAsync(_user.Id);
    Assert.NotNull(user);
    Assert.Equal(_user.FirstName, user.FirstName);
    Assert.Equal(_user.MiddleName, user.MiddleName);
    Assert.Equal(_user.LastName, user.LastName);
    Assert.Equal(_user.Nickname, user.Nickname);
    Assert.Equal(_user.Birthdate, user.Birthdate);
    Assert.Equal(_user.Gender, user.Gender);
    Assert.Equal(_user.Locale, user.Locale);
    Assert.Equal(_user.TimeZone, user.TimeZone);
    Assert.Equal(_user.Picture, user.Picture);
    Assert.Equal(_user.Profile, user.Profile);
    Assert.Equal(_user.Website, user.Website);
    Assert.Equal(_user.CustomAttributes, user.CustomAttributes);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified users.")]
  public async Task SaveAsync_it_should_save_the_specified_users()
  {
    UserAggregate disabled = new(new UniqueNameUnit(_userSettings.UniqueName, "disabled"));
    UserAggregate deleted = new(new UniqueNameUnit(_userSettings.UniqueName, "deleted"));
    await _userRepository.SaveAsync([disabled, deleted]);

    Dictionary<string, UserEntity> entities = await IdentityContext.Users.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(disabled.Id.Value));
    Assert.True(entities.ContainsKey(deleted.Id.Value));

    disabled.Disable();
    deleted.Delete();
    await _userRepository.SaveAsync([disabled, deleted]);

    entities = await IdentityContext.Users.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(disabled.Id.Value));
    Assert.False(entities.ContainsKey(deleted.Id.Value));

    AssertUsers.AreEqual(disabled, entities[disabled.Id.Value]);

    ActorEntity? actor = await IdentityContext.Actors.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == deleted.Id.Value);
    Assert.NotNull(actor);
    Assert.True(actor.IsDeleted);
  }

  public Task DisposeAsync() => Task.CompletedTask;
}
