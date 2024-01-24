using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

public class SessionRepositoryTests : RepositoryTests, IAsyncLifetime
{
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;
  private readonly IUserSettings _userSettings;

  private readonly UserAggregate _user;
  private readonly SessionAggregate _session;

  public SessionRepositoryTests() : base()
  {
    _sessionRepository = ServiceProvider.GetRequiredService<ISessionRepository>();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    _userSettings = ServiceProvider.GetRequiredService<IUserSettingsResolver>().Resolve();

    UserId userId = UserId.NewId();
    ActorId actorId = new(userId.Value);

    TenantId tenantId = new("tests");
    UniqueNameUnit uniqueName = new(_userSettings.UniqueName, Faker.Person.UserName);
    _user = new(uniqueName, tenantId, actorId, userId);

    _session = new(_user);

    _session.SetCustomAttribute("IpAddress", Faker.Internet.Ip());
    _session.Update(actorId);
  }

  public async Task InitializeAsync()
  {
    await EventContext.Database.MigrateAsync();
    await IdentityContext.Database.MigrateAsync();

    TableId[] tables = [IdentityDb.Sessions.Table, IdentityDb.Users.Table, IdentityDb.CustomAttributes.Table, IdentityDb.Actors.Table, EventDb.Events.Table];
    foreach (TableId table in tables)
    {
      ICommand command = SqlServerDeleteBuilder.From(table).Build();
      await IdentityContext.Database.ExecuteSqlRawAsync(command.Text, command.Parameters.ToArray());
    }

    await _userRepository.SaveAsync(_user);
    await _sessionRepository.SaveAsync(_session);
  }

  [Fact(DisplayName = "LoadAsync: it should load all the sessions.")]
  public async Task LoadAsync_it_should_load_all_the_sessions()
  {
    SessionAggregate deleted = new(_user);
    deleted.Delete();
    await _sessionRepository.SaveAsync(deleted);

    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(includeDeleted: true);
    Assert.Equal(2, sessions.Count());
    Assert.Contains(sessions, _session.Equals);
    Assert.Contains(sessions, deleted.Equals);
  }

  [Fact(DisplayName = "LoadAsync: it should load the active sessions by user.")]
  public async Task LoadAsync_it_should_load_the_active_sessions_by_user()
  {
    UserAggregate user = new(_user.UniqueName, tenantId: null);
    await _userRepository.SaveAsync(user);

    SessionAggregate session = new(user);
    SessionAggregate deleted = new(_user);
    deleted.Delete();
    SessionAggregate signedOut = new(_user);
    signedOut.SignOut();
    await _sessionRepository.SaveAsync([session, deleted, signedOut]);
    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadActiveAsync(_user);
    Assert.Equal(_session, sessions.Single());
  }

  [Fact(DisplayName = "LoadAsync: it should load the session by identifier.")]
  public async Task LoadAsync_it_should_load_the_session_by_identifier()
  {
    Assert.Null(await _sessionRepository.LoadAsync(SessionId.NewId()));

    _session.Delete();
    long version = _session.Version;

    _session.SignOut();
    await _sessionRepository.SaveAsync(_session);

    Assert.Null(await _sessionRepository.LoadAsync(_session.Id, version));

    SessionAggregate? session = await _sessionRepository.LoadAsync(_session.Id, version, includeDeleted: true);
    Assert.NotNull(session);
    Assert.True(session.IsActive);
    Assert.Equal(_session, session);
  }

  [Fact(DisplayName = "LoadAsync: it should load the sessions by tenant identifier.")]
  public async Task LoadAsync_it_should_load_the_sessions_by_tenant_identifier()
  {
    UserAggregate user = new(_user.UniqueName, tenantId: null);
    await _userRepository.SaveAsync(user);

    SessionAggregate session = new(user);
    SessionAggregate deleted = new(_user);
    deleted.Delete();
    await _sessionRepository.SaveAsync([session, deleted]);

    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(_user.TenantId);
    Assert.Equal(_session, sessions.Single());
  }

  [Fact(DisplayName = "LoadAsync: it should load the sessions by user.")]
  public async Task LoadAsync_it_should_load_the_sessions_by_user()
  {
    UserAggregate user = new(_user.UniqueName, tenantId: null);
    await _userRepository.SaveAsync(user);

    SessionAggregate session = new(user);
    SessionAggregate deleted = new(_user);
    deleted.Delete();
    SessionAggregate signedOut = new(_user);
    signedOut.SignOut();
    await _sessionRepository.SaveAsync([session, deleted, signedOut]);

    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(_user);
    Assert.Equal(2, sessions.Count());
    Assert.Contains(sessions, _session.Equals);
    Assert.Contains(sessions, signedOut.Equals);
  }

  [Fact(DisplayName = "LoadAsync: it should load the sessions by identifiers.")]
  public async Task LoadAsync_it_should_load_the_sessions_by_identifiers()
  {
    SessionAggregate deleted = new(_user);
    deleted.Delete();
    await _sessionRepository.SaveAsync(deleted);

    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync([_session.Id, deleted.Id, SessionId.NewId()], includeDeleted: true);
    Assert.Equal(2, sessions.Count());
    Assert.Contains(sessions, _session.Equals);
    Assert.Contains(sessions, deleted.Equals);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified session.")]
  public async Task SaveAsync_it_should_save_the_specified_session()
  {
    SessionEntity? entity = await IdentityContext.Sessions.AsNoTracking()
      .Include(x => x.User)
      .SingleOrDefaultAsync(x => x.AggregateId == _session.Id.Value);
    Assert.NotNull(entity);
    AssertSessions.AreEqual(_session, entity);

    Dictionary<string, string> customAttributes = await IdentityContext.CustomAttributes.AsNoTracking()
      .Where(x => x.EntityType == nameof(IdentityContext.Sessions) && x.EntityId == entity.SessionId)
      .ToDictionaryAsync(x => x.Key, x => x.Value);
    Assert.Equal(_session.CustomAttributes, customAttributes);
  }

  [Fact(DisplayName = "SaveAsync: it should save the specified sessions.")]
  public async Task SaveAsync_it_should_save_the_specified_sessions()
  {
    SessionAggregate signedOut = new(_user);
    SessionAggregate deleted = new(_user);
    await _sessionRepository.SaveAsync([signedOut, deleted]);

    Dictionary<string, SessionEntity> entities = await IdentityContext.Sessions.AsNoTracking().ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(signedOut.Id.Value));
    Assert.True(entities.ContainsKey(deleted.Id.Value));

    signedOut.SignOut();
    deleted.Delete();
    await _sessionRepository.SaveAsync([signedOut, deleted]);

    entities = await IdentityContext.Sessions.AsNoTracking().Include(x => x.User).ToDictionaryAsync(x => x.AggregateId, x => x);
    Assert.True(entities.ContainsKey(signedOut.Id.Value));
    Assert.False(entities.ContainsKey(deleted.Id.Value));

    AssertSessions.AreEqual(signedOut, entities[signedOut.Id.Value]);
  }

  public Task DisposeAsync() => Task.CompletedTask;
}
