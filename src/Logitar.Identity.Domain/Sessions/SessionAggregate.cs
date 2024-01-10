using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Sessions;

public class SessionAggregate : AggregateRoot
{
  private SessionUpdatedEvent _updatedEvent = new();

  public new SessionId Id => new(base.Id);

  private UserId? _userId = null;
  public UserId UserId => _userId ?? throw new InvalidOperationException($"The {nameof(UserId)} has not been initialized yet.");

  private Password? _secret = null;
  public bool IsPersistent => _secret != null;

  public bool IsActive { get; private set; }

  private readonly Dictionary<string, string> _customAttributes = [];
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  public SessionAggregate(AggregateId id) : base(id)
  {
  }

  public SessionAggregate(UserAggregate user, Password? secret = null, ActorId actorId = default, SessionId? id = null)
    : base((id ?? SessionId.NewId()).AggregateId)
  {
    Raise(new SessionCreatedEvent(actorId, secret, user.Id));
  }
  protected virtual void Apply(SessionCreatedEvent @event)
  {
    _userId = @event.UserId;

    _secret = @event.Secret;

    IsActive = true;
  }

  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new SessionDeletedEvent(actorId));
    }
  }

  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();

    if (_customAttributes.ContainsKey(key))
    {
      _updatedEvent.CustomAttributes[key] = null;
      _customAttributes.Remove(key);
    }
  }

  public void Renew(byte[] currentSecret, Password newSecret, ActorId actorId = default)
  {
    if (!IsActive)
    {
      throw new SessionIsNotActiveException(this);
    }
    else if (_secret == null)
    {
      throw new SessionIsNotPersistentException(this);
    }
    else if (!_secret.IsMatch(currentSecret))
    {
      throw new IncorrectSessionSecretException(this, currentSecret);
    }

    Raise(new SessionRenewedEvent(actorId, newSecret));
  }
  protected virtual void Apply(SessionRenewedEvent @event)
  {
    _secret = @event.Secret;
  }

  private readonly CustomAttributeValidator _customAttributeValidator = new();
  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    _customAttributeValidator.ValidateAndThrow(key, value);

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || existingValue != value)
    {
      _updatedEvent.CustomAttributes[key] = value;
      _customAttributes[key] = value;
    }
  }

  public void SignOut(ActorId actorId = default)
  {
    if (IsActive)
    {
      Raise(new SessionSignedOutEvent(actorId));
    }
  }
  protected virtual void Apply(SessionSignedOutEvent _)
  {
    IsActive = false;
  }

  public void Update(ActorId actorId = default)
  {
    if (_updatedEvent.HasChanges)
    {
      _updatedEvent.ActorId = actorId;
      Raise(_updatedEvent);
      _updatedEvent = new();
    }
  }
  protected virtual void Apply(SessionUpdatedEvent @event)
  {
    foreach (KeyValuePair<string, string?> custonAttribute in @event.CustomAttributes)
    {
      if (custonAttribute.Value == null)
      {
        _customAttributes.Remove(custonAttribute.Key);
      }
      else
      {
        _customAttributes[custonAttribute.Key] = custonAttribute.Value;
      }
    }
  }
}
