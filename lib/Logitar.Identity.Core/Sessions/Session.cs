using Logitar.EventSourcing;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Sessions.Events;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core.Sessions;

/// <summary>
/// Represents a session in the identity system. A session allows an user to perform actions in a timeframe.
/// It can be signed-out to close the timeframe, or renewed to extend the timeframe.
/// </summary>
public class Session : AggregateRoot
{
  /// <summary>
  /// The updated event.
  /// </summary>
  private SessionUpdated _updated = new();

  /// <summary>
  /// Gets the identifier of the session.
  /// </summary>
  public new UserId Id => new(base.Id);
  /// <summary>
  /// Gets the tenant identifier of the session.
  /// </summary>
  public TenantId? TenantId => Id.TenantId;
  /// <summary>
  /// Gets the entity identifier of the session. This identifier is unique within the tenant.
  /// </summary>
  public EntityId? EntityId => Id.EntityId;

  /// <summary>
  /// The identifier of the user owning the session.
  /// </summary>
  private UserId? _userId = null;
  /// <summary>
  /// Gets the identifier of the user owning the session.
  /// </summary>
  /// <exception cref="InvalidOperationException">The user identifier has not been initialized yet.</exception>
  public UserId UserId => _userId ?? throw new InvalidOperationException($"The {nameof(UserId)} has not been initialized yet.");

  /// <summary>
  /// The secret of the session.
  /// </summary>
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

  /// <summary>
  /// The custom attributes of the session.
  /// </summary>
  private readonly Dictionary<Identifier, string> _customAttributes = [];
  /// <summary>
  /// Gets the custom attributes of the session.
  /// </summary>
  public IReadOnlyDictionary<Identifier, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Initializes a new instance of the <see cref="Session"/> class.
  /// DO NOT use this constructor to create a new session. It is only intended to be used by the event sourcing.
  /// </summary>
  public Session() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Session"/> class.
  /// DO use this constructor to create a new user.
  /// </summary>
  /// <param name="user">The user owning the session.</param>
  /// <param name="secret">The secret of the session.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <param name="id">The identifier of the session.</param>
  /// <exception cref="TenantMismatchException">The user and session tenant identifiers do not match.</exception>
  public Session(User user, Password? secret = null, ActorId? actorId = null, SessionId? id = null) : base((id ?? SessionId.NewId()).StreamId)
  {
    if (id.HasValue && id.Value.TenantId != user.TenantId)
    {
      throw new TenantMismatchException(id.Value.TenantId, user.TenantId);
    }

    actorId ??= new(user.Id.Value);
    Raise(new SessionCreated(secret, user.Id), actorId.Value);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(SessionCreated @event)
  {
    _userId = @event.UserId;

    _secret = @event.Secret;

    IsActive = true;
  }

  /// <summary>
  /// Deletes the session, if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId? actorId = null)
  {
    if (!IsDeleted)
    {
      Raise(new SessionDeleted(), actorId);
    }
  }

  /// <summary>
  /// Removes the specified custom attribute on the session.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(Identifier key)
  {
    if (_customAttributes.Remove(key))
    {
      _updated.CustomAttributes[key] = null;
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
    Raise(new SessionRenewed(newSecret), actorId.Value);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(SessionRenewed @event)
  {
    _secret = @event.Secret;
  }

  /// <summary>
  /// Sets the specified custom attribute on the session.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  public void SetCustomAttribute(Identifier key, string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      RemoveCustomAttribute(key);
    }
    value = value.Trim();

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || existingValue != value)
    {
      _customAttributes[key] = value;
      _updated.CustomAttributes[key] = value;
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
      Raise(new SessionSignedOut(), actorId.Value);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Handle(SessionSignedOut _)
  {
    IsActive = false;
  }

  /// <summary>
  /// Applies updates on the session.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Update(ActorId? actorId = null)
  {
    if (_updated.HasChanges)
    {
      Raise(_updated, actorId, DateTime.Now);
      _updated = new();
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(SessionUpdated @event)
  {
    foreach (KeyValuePair<Identifier, string?> customAttribute in @event.CustomAttributes)
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
