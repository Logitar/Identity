using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.Infrastructure.Passwords.Pbkdf2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

[Trait(Traits.Category, Categories.Integration)]
public class SessionPostgresIntegrationTests : IntegrationTests
{
  private const string SecretString = "P@s$W0rD";

  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;

  private readonly ActorId _actorId = ActorId.NewId();
  private readonly Password _secret;
  private readonly UniqueNameSettings _uniqueNameSettings = new();

  public SessionPostgresIntegrationTests() : base(DatabaseProvider.PostgreSQL)
  {
    _sessionRepository = ServiceProvider.GetRequiredService<ISessionRepository>();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();

    Pbkdf2Settings pbkdf2 = new();
    _secret = new Pbkdf2Password(SecretString, pbkdf2.Algorithm, pbkdf2.Iterations, pbkdf2.SaltLength, pbkdf2.HashLength);
  }

  [Theory(DisplayName = "LoadActiveAsync: it should return the correct result, given a user.")]
  [InlineData(false)]
  [InlineData(true)]
  public async Task GivenUser_When_LoadActiveAsync_Then_CorrectResults(bool found)
  {
    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    await _userRepository.SaveAsync(user);

    Session session = new(user);
    Session signedOut = new(user);
    signedOut.SignOut();
    if (found)
    {
      await _sessionRepository.SaveAsync([session, signedOut]);
    }

    IReadOnlyCollection<Session> sessions = await _sessionRepository.LoadActiveAsync(user);

    if (found)
    {
      Assert.Equal(session, Assert.Single(sessions));
    }
    else
    {
      Assert.Empty(sessions);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a deletion status.")]
  [InlineData(null)]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_Deleted_When_LoadAsync_Then_CorrectResult(bool? isDeleted)
  {
    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    await _userRepository.SaveAsync(user);

    Session session1 = new(user);
    Session session2 = new(user);
    session2.Delete();
    Session session3 = new(user);
    if (isDeleted == true)
    {
      session3.Delete();
    }
    if (isDeleted.HasValue)
    {
      await _sessionRepository.SaveAsync([session1, session2, session3]);
    }

    IReadOnlyCollection<Session> sessions = await _sessionRepository.LoadAsync(isDeleted);

    if (isDeleted.HasValue)
    {
      Assert.Equal(2, sessions.Count);
      Assert.Contains(session3, sessions);
      Assert.Contains(isDeleted.Value ? session2 : session1, sessions);
    }
    else
    {
      Assert.Empty(sessions);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given IDs and a deletion status.")]
  [InlineData(null)]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_IdsDeleted_When_LoadAsync_Then_CorrectResult(bool? isDeleted)
  {
    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    await _userRepository.SaveAsync(user);

    Session session1 = new(user);
    Session session2 = new(user);
    session2.Delete();
    Session session3 = new(user);
    if (isDeleted == true)
    {
      session3.Delete();
    }
    Session session4 = new(user);
    if (isDeleted == true)
    {
      session4.Delete();
    }
    if (isDeleted.HasValue)
    {
      await _sessionRepository.SaveAsync([session1, session2, session3, session4]);
    }

    IReadOnlyCollection<Session> sessions = await _sessionRepository.LoadAsync([session1.Id, session2.Id, session3.Id, SessionId.NewId()], isDeleted);

    if (isDeleted.HasValue)
    {
      Assert.Equal(2, sessions.Count);
      Assert.Contains(session3, sessions);
      Assert.Contains(isDeleted.Value ? session2 : session1, sessions);
    }
    else
    {
      Assert.Empty(sessions);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given an ID, a version and a deletion status.")]
  [InlineData(false, null, false)]
  [InlineData(true, null, false)]
  [InlineData(true, false, false)]
  [InlineData(true, true, true)]
  public async Task Given_IdVersionDeleted_When_LoadAsync_Then_CorrectResult(bool found, bool? isDeleted, bool withVersion)
  {
    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    await _userRepository.SaveAsync(user);

    Session session = new(user);
    if (isDeleted == true)
    {
      session.Delete();
    }
    if (found)
    {
      await _sessionRepository.SaveAsync(session);
    }

    long? version = withVersion ? 1 : null;
    Session? result = await _sessionRepository.LoadAsync(session.Id, version, isDeleted);
    if (found)
    {
      Assert.NotNull(result);
      if (withVersion)
      {
        Assert.Equal(version, result.Version);
        Assert.False(result.IsDeleted);
      }
      else
      {
        Assert.Equal(session.Version, result.Version);
        Assert.Equal(isDeleted ?? false, result.IsDeleted);
      }
    }
    else
    {
      Assert.Null(result);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a tenant ID.")]
  [InlineData(false, null)]
  [InlineData(true, null)]
  [InlineData(true, "d1d97022-f092-4bfa-9866-bfdfe36fd8a7")]
  public async Task Given_TenantId_When_LoadAsync_Then_CorrectResults(bool found, string? tenantIdValue)
  {
    TenantId tenantId = tenantIdValue == null ? TenantId.NewId() : new(tenantIdValue);

    User user1 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    User user2 = new(user1.UniqueName, actorId: null, UserId.NewId(tenantId));
    await _userRepository.SaveAsync([user1, user2]);

    Session session1 = new(user1);
    Session session2 = new(user2, secret: null, actorId: null, SessionId.NewId(tenantId));
    if (found)
    {
      await _sessionRepository.SaveAsync([session1, session2]);
    }

    IReadOnlyCollection<Session> sessions = await _sessionRepository.LoadAsync(tenantIdValue == null ? null : tenantId);

    if (found)
    {
      Assert.Equal(tenantIdValue == null ? session1 : session2, Assert.Single(sessions));
    }
    else
    {
      Assert.Empty(sessions);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a user.")]
  [InlineData(false)]
  [InlineData(true)]
  public async Task GivenUser_When_LoadAsync_Then_CorrectResults(bool found)
  {
    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    await _userRepository.SaveAsync(user);

    Session session = new(user);
    if (found)
    {
      await _sessionRepository.SaveAsync(session);
    }

    IReadOnlyCollection<Session> sessions = await _sessionRepository.LoadAsync(user);

    if (found)
    {
      Assert.Equal(session, Assert.Single(sessions));
    }
    else
    {
      Assert.Empty(sessions);
    }
  }

  [Fact(DisplayName = "SaveAsync: it should save the session correctly.")]
  public async Task Given_Session_When_SaveAsync_Then_SavedCorrectly()
  {
    TenantId tenantId = TenantId.NewId();

    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName), actorId: null, UserId.NewId(tenantId));
    await _userRepository.SaveAsync(user);

    Session session = new(user, _secret, _actorId, SessionId.NewId(tenantId));
    string ip = Faker.Internet.Ip();
    session.SetCustomAttribute(new Identifier("IpAddress"), ip);
    session.Update();
    session.Renew(SecretString, _secret);
    session.SignOut(_actorId);
    await _sessionRepository.SaveAsync(session);

    SessionEntity? entity = await IdentityContext.Sessions.AsNoTracking()
      .Include(x => x.User)
      .SingleOrDefaultAsync();

    Assert.NotNull(entity);
    Assert.Equal(session.Id.Value, entity.StreamId);
    Assert.Equal(session.Version, entity.Version);
    Assert.Equal(session.CreatedBy?.Value, entity.CreatedBy);
    Assert.Equal(session.CreatedOn.AsUniversalTime(), entity.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(session.UpdatedBy?.Value, entity.UpdatedBy);
    Assert.Equal(session.UpdatedOn.AsUniversalTime(), entity.UpdatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(tenantId.Value, entity.TenantId);
    Assert.Equal(session.EntityId.Value, entity.EntityId);
    Assert.Equal(user.Id.Value, entity.User?.StreamId);
    Assert.Equal(_secret.Encode(), entity.SecretHash);
    Assert.True(entity.IsPersistent);
    Assert.Equal(_actorId.Value, entity.SignedOutBy);
    Assert.True(entity.SignedOutOn.HasValue);
    Assert.Equal(DateTime.UtcNow, entity.SignedOutOn.Value, TimeSpan.FromSeconds(1));
    Assert.False(session.IsActive);
    Assert.Equal($@"{{""IpAddress"":""{ip}""}}", entity.CustomAttributes);

    CustomAttributeEntity[] customAttributes = await IdentityContext.CustomAttributes.AsNoTracking().ToArrayAsync();
    Assert.Equal(session.CustomAttributes.Count, customAttributes.Length);
    foreach (KeyValuePair<Identifier, string> customAttribute in session.CustomAttributes)
    {
      Assert.Contains(customAttributes, c => c.EntityType == EntityType.Session && c.EntityId == entity.SessionId && c.Key == customAttribute.Key.Value
        && c.Value == customAttribute.Value && c.ValueShortened == customAttribute.Value.Truncate(byte.MaxValue));
    }

    session.Delete();
    await _sessionRepository.SaveAsync(session);

    entity = await IdentityContext.Sessions.AsNoTracking().SingleOrDefaultAsync();
    Assert.Null(entity);

    customAttributes = await IdentityContext.CustomAttributes.AsNoTracking().ToArrayAsync();
    Assert.Empty(customAttributes);
  }
}
