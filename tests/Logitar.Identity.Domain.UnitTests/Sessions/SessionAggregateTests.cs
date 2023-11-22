using Bogus;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Domain.Sessions;

[Trait(Traits.Category, Categories.Unit)]
public class SessionAggregateTests
{
  private readonly Faker _faker = new();
  private readonly UserAggregate _user;
  private readonly SessionAggregate _session;

  public SessionAggregateTests()
  {
    _user = new(new UniqueNameUnit(new UniqueNameSettings(), _faker.Person.UserName));
    _session = new(_user);
  }

  [Fact(DisplayName = "ctor: it should create a new session using the specified actor identifier.")]
  public void ctor_it_should_create_a_new_session_using_the_specified_actor_identifier()
  {
    ActorId actorId = ActorId.NewId();
    SessionAggregate session = new(_user, actorId: actorId);
    Assert.Contains(session.Changes, change => change is SessionCreatedEvent @event && @event.ActorId == actorId);
  }

  [Fact(DisplayName = "ctor: it should create a new session with id.")]
  public void ctor_it_should_create_a_new_session_with_id()
  {
    SessionId sessionId = new(AggregateId.NewId());

    SessionAggregate session = new(sessionId.AggregateId);

    Assert.Equal(sessionId, session.Id);
    Assert.Equal(sessionId.AggregateId, session.Id.AggregateId);
    Assert.False(session.IsActive);
    Assert.False(session.IsPersistent);
  }

  [Fact(DisplayName = "ctor: it should create a new session with parameters.")]
  public void ctor_it_should_create_a_new_session_with_parameters()
  {
    string secretString = RandomStringGenerator.GetString(32);
    PasswordMock secret = new(secretString);
    SessionId id = new(Guid.NewGuid().ToString());

    SessionAggregate session = new(_user, secret, actorId: null, id);

    Assert.Equal(id, session.Id);
    Assert.Equal(_user.Id.Value, session.CreatedBy.Value);
    Assert.True(session.IsActive);
    Assert.True(session.IsPersistent);
    Assert.Equal(_user.Id, session.UserId);
    AssertSecret(session, secretString);
  }

  [Fact(DisplayName = "Delete: it should delete the session when it is not deleted.")]
  public void Delete_it_should_delete_the_session_when_it_is_not_deleted()
  {
    Assert.False(_session.IsDeleted);

    _session.Delete();
    Assert.True(_session.IsDeleted);
    Assert.Contains(_session.Changes, change => change is SessionDeletedEvent);

    _session.ClearChanges();
    _session.Delete();
    Assert.False(_session.HasChanges);
  }

  [Fact(DisplayName = "RemoveCustomAttribute: it should remove an existing custom attribute.")]
  public void RemoveCustomAttribute_it_should_remove_an_existing_custom_attribute()
  {
    string key = "remove_sessions";

    _session.SetCustomAttribute(key, bool.TrueString);
    _session.Update();

    _session.RemoveCustomAttribute($"  {key}  ");
    _session.Update();
    Assert.False(_session.CustomAttributes.ContainsKey(key));

    _session.RemoveCustomAttribute(key);
    AssertHasNoUpdate(_session);
  }

  [Fact(DisplayName = "Renew: it should renew the session.")]
  public void Renew_it_should_renew_the_session()
  {
    string oldSecretString = RandomStringGenerator.GetString(32);
    PasswordMock oldSecret = new(oldSecretString);
    SessionAggregate session = new(_user, oldSecret);

    string newSecretString = RandomStringGenerator.GetString(32);
    PasswordMock newSecret = new(newSecretString);

    session.Renew(oldSecretString, newSecret);
    Assert.Contains(session.Changes, change => change is SessionRenewedEvent @event
      && @event.ActorId.Value == _user.Id.Value
      && @event.Secret == newSecret);
  }

  [Fact(DisplayName = "Renew: it should renew the session using the specified actor identifier.")]
  public void Renew_it_should_renew_the_session_using_the_specified_actor_identifier()
  {
    string secretString = RandomStringGenerator.GetString(32);
    PasswordMock secret = new(secretString);
    SessionAggregate session = new(_user, secret);

    ActorId actorId = ActorId.NewId();
    session.Renew(secretString, secret, actorId: actorId);
    Assert.Contains(session.Changes, change => change is SessionRenewedEvent @event && @event.ActorId == actorId);
  }

  [Fact(DisplayName = "Renew: it should throw IncorrectSessionSecretException when the current secret is incorrect.")]
  public void Renew_it_should_throw_IncorrectSessionSecretException_when_the_current_secret_is_incorrect()
  {
    string secretString = RandomStringGenerator.GetString(32);
    PasswordMock secret = new(secretString);
    SessionAggregate session = new(_user, secret);

    string attemptedSecret = secretString[1..];
    var exception = Assert.Throws<IncorrectSessionSecretException>(() => session.Renew(attemptedSecret, secret));
    Assert.Equal(attemptedSecret, exception.AttemptedSecret);
    Assert.Equal(session.Id, exception.SessionId);
  }

  [Fact(DisplayName = "Renew: it should throw IncorrectSessionSecretException when the session has no secret.")]
  public void Renew_it_should_throw_IncorrectSessionSecretException_when_the_session_has_no_secret()
  {
    string secretString = RandomStringGenerator.GetString(32);
    PasswordMock newSecret = new(secretString);

    var exception = Assert.Throws<IncorrectSessionSecretException>(() => _session.Renew(secretString, newSecret));
    Assert.Equal(secretString, exception.AttemptedSecret);
    Assert.Equal(_session.Id, exception.SessionId);
  }

  [Fact(DisplayName = "Renew: it should throw SessionIsNotActiveException when the session is not active.")]
  public void Renew_it_should_throw_SessionIsNotActiveException_when_the_session_is_not_active()
  {
    string currentSecret = RandomStringGenerator.GetString(32);
    PasswordMock newSecret = new(currentSecret);
    SessionAggregate session = new(_user, newSecret);

    session.SignOut();

    var exception = Assert.Throws<SessionIsNotActiveException>(() => session.Renew(currentSecret, newSecret));
    Assert.Equal(session.Id, exception.SessionId);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should set a custom attribute when it is different.")]
  public void SetCustomAttribute_it_should_set_a_custom_attribute_when_it_is_different()
  {
    string key = "  remove_sessions  ";
    string value = $"  {bool.TrueString}  ";
    _session.SetCustomAttribute(key, value);
    Assert.Equal(value.Trim(), _session.CustomAttributes[key.Trim()]);

    _session.Update();

    _session.SetCustomAttribute(key, value);
    AssertHasNoUpdate(_session);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should throw ValidationException when the key or value is not valid.")]
  public void SetCustomAttribute_it_should_throw_ValidationException_when_the_key_or_value_is_not_valid()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _session.SetCustomAttribute("   ", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    string key = _faker.Random.String(CustomAttributeKeyValidator.MaximumLength + 1, minChar: 'A', maxChar: 'Z');
    exception = Assert.Throws<FluentValidation.ValidationException>(() => _session.SetCustomAttribute(key, "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _session.SetCustomAttribute("-key", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _session.SetCustomAttribute("key", "     "));
    Assert.All(exception.Errors, e => Assert.Equal("Value", e.PropertyName));
  }

  [Fact(DisplayName = "SignOut: it should sign-out the session if it is active.")]
  public void SignOut_it_should_sign_out_the_session_if_it_is_active()
  {
    Assert.True(_session.IsActive);

    _session.SignOut();
    Assert.False(_session.IsActive);
    Assert.Contains(_session.Changes, change => change is SessionSignedOutEvent @event && @event.ActorId.Value == _user.Id.Value);

    _session.ClearChanges();

    _session.SignOut();
    Assert.False(_session.HasChanges);
  }

  [Fact(DisplayName = "SignOut: it should sign-out the session using the specified actor identifier.")]
  public void SignOut_it_should_sign_out_the_session_using_the_specified_actor_identifier()
  {
    ActorId actorId = ActorId.NewId();
    _session.SignOut(actorId);
    Assert.Contains(_session.Changes, change => change is SessionSignedOutEvent @event && @event.ActorId == actorId);
  }

  [Fact(DisplayName = "Update: it should update the session when it has changes.")]
  public void Update_it_should_update_the_session_when_it_has_changes()
  {
    ActorId actorId = ActorId.NewId();

    _session.SetCustomAttribute("IpAddress", _faker.Internet.IpAddress().ToString());
    _session.Update(actorId);
    Assert.Equal(actorId, _session.UpdatedBy);

    long version = _session.Version;
    _session.Update(actorId);
    Assert.Equal(version, _session.Version);
  }

  private static void AssertHasNoUpdate(SessionAggregate session)
  {
    FieldInfo? field = session.GetType().GetField("_updated", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    SessionUpdatedEvent? updated = field.GetValue(session) as SessionUpdatedEvent;
    Assert.NotNull(updated);
    Assert.False(updated.HasChanges);
  }
  private static void AssertSecret(SessionAggregate session, string? secret)
  {
    FieldInfo? field = session.GetType().GetField("_secret", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    Password? instance = field.GetValue(session) as Password;
    if (secret == null)
    {
      Assert.Null(instance);
    }
    else
    {
      Assert.NotNull(instance);
      Assert.True(instance.IsMatch(secret));
    }
  }
}
