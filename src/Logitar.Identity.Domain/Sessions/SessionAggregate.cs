using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Sessions;

/// <summary>
/// Represents a session in the identity system. A session allows an user to perform actions in a timeframe.
/// It can be signed-out to close the timeframe, or renewed to extend the timeframe.
/// </summary>
public class SessionAggregate : AggregateRoot
{
  private SessionUpdatedEvent _updatedEvent = new();

  /// <summary>
  /// Gets the identifier of the session.
  /// </summary>
  public new SessionId Id => new(base.Id);

  private UserId? _userId = null;
  /// <summary>
  /// Gets the identifier of the user owning the session.
  /// </summary>
  /// <exception cref="InvalidOperationException">The user identifier has not been initialized yet.</exception>
  public UserId UserId => _userId ?? throw new InvalidOperationException($"The {nameof(UserId)} has not been initialized yet.");

  private Password? _secret = null;
  /// <summary>
  /// Gets a value indicating whether or not the session is persistent.
  /// A persistent session has a secret that can be used with a refresh token to renew the session.
  /// </summary>
  public bool IsPersistent => _secret != null;

  /// <summary>
  /// Gets or sets a value indicating whether or not the session is still active.
  /// A session becomes inactive once it is signed-out.
  /// </summary>
  public bool IsActive { get; private set; }

  private readonly Dictionary<string, string> _customAttributes = [];
  /// <summary>
  /// Gets the custom attributes of the session.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionAggregate"/> class.
  /// DO NOT use this constructor to create a new session. It is only intended to be used by the event sourcing.
  /// </summary>
  /// <param name="id">The identifier of the session.</param>
  public SessionAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionAggregate"/> class.
  /// DO use this constructor to create a new user.
  /// </summary>
  /// <param name="user">The user owning the session.</param>
  /// <param name="secret">The secret of the session.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <param name="id">The identifier of the session.</param>
  public SessionAggregate(UserAggregate user, Password? secret = null, ActorId? actorId = null, SessionId? id = null)
    : base((id ?? SessionId.NewId()).AggregateId)
  {
    actorId ??= new(user.Id.Value);
    Raise(new SessionCreatedEvent(actorId.Value, secret, user.Id));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(SessionCreatedEvent @event)
  {
    _userId = @event.UserId;

    _secret = @event.Secret;

    IsActive = true;
  }

  /// <summary>
  /// Deletes the session, if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new SessionDeletedEvent(actorId));
    }
  }

  /// <summary>
  /// Removes the specified custom attribute on the session.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();

    if (_customAttributes.ContainsKey(key))
    {
      _updatedEvent.CustomAttributes[key] = null;
      _customAttributes.Remove(key);
    }
  }

  /// <summary>
  /// Renews the session.
  /// </summary>
  /// <param name="currentSecret">The current secret of the session.</param>
  /// <param name="newSecret">The new secret of the session.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <exception cref="IncorrectSessionSecretException">The current secret is incorrect.</exception>
  /// <exception cref="SessionIsNotActiveException">The session is not active.</exception>
  /// <exception cref="SessionIsNotPersistentException">The session is not persistent.</exception>
  public void Renew(string currentSecret, Password newSecret, ActorId? actorId = default)
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

    actorId ??= new(UserId.Value);
    Raise(new SessionRenewedEvent(actorId.Value, newSecret));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(SessionRenewedEvent @event)
  {
    _secret = @event.Secret;
  }

  private readonly CustomAttributeValidator _customAttributeValidator = new();
  /// <summary>
  /// Sets the specified custom attribute on the session.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
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

  /// <summary>
  /// Signs-out the session.
  /// </summary>
  /// <param name="actorId">The actor identifier. If left null, the user identifier will be used as the actor identifier.</param>
  public void SignOut(ActorId? actorId = default)
  {
    if (IsActive)
    {
      actorId ??= new(UserId.Value);
      Raise(new SessionSignedOutEvent(actorId.Value));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Apply(SessionSignedOutEvent _)
  {
    IsActive = false;
  }

  /// <summary>
  /// Applies updates on the session.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Update(ActorId actorId = default)
  {
    if (_updatedEvent.HasChanges)
    {
      _updatedEvent.ActorId = actorId;
      Raise(_updatedEvent);
      _updatedEvent = new();
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(SessionUpdatedEvent @event)
  {
    foreach (KeyValuePair<string, string?> customAttribute in @event.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        _customAttributes.Remove(customAttribute.Key);
      }
      else
      {
        _customAttributes[customAttribute.Key] = customAttribute.Value;
      }
    }
  }
}
