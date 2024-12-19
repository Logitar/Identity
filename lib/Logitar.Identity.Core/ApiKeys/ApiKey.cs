using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Events;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Roles;

namespace Logitar.Identity.Core.ApiKeys;

/// <summary>
/// Represents an API key in the identity system. Similarly to an user, it can have access to processes and resources using roles.
/// It should be used instead of users for backend-to-backend authentication where personal and authentication information are not required.
/// </summary>
public class ApiKey : AggregateRoot
{
  /// <summary>
  /// The updated event.
  /// </summary>
  private ApiKeyUpdated _updated = new();

  /// <summary>
  /// The API key secret.
  /// </summary>
  private Password? _secret = null;

  /// <summary>
  /// Gets the identifier of the API key.
  /// </summary>
  public new ApiKeyId Id => new(base.Id);
  /// <summary>
  /// Gets the tenant identifier of the API key.
  /// </summary>
  public TenantId? TenantId => Id.TenantId;
  /// <summary>
  /// Gets the entity identifier of the API key. This identifier is unique within the tenant.
  /// </summary>
  public EntityId? EntityId => Id.EntityId;

  /// <summary>
  /// The display name of the API key.
  /// </summary>
  private DisplayName? _displayName = null;
  /// <summary>
  /// Gets or sets the display name of the API key.
  /// </summary>
  /// <exception cref="InvalidOperationException">The display name has not been initialized yet.</exception>
  public DisplayName DisplayName
  {
    get => _displayName ?? throw new InvalidOperationException($"The {nameof(DisplayName)} has not been initialized yet.");
    set
    {
      if (_displayName != value)
      {
        _displayName = value;
        _updated.DisplayName = value;
      }
    }
  }
  /// <summary>
  /// The description of the API key.
  /// </summary>
  private Description? _description = null;
  /// <summary>
  /// Gets or sets the description of the API key.
  /// </summary>
  public Description? Description
  {
    get => _description;
    set
    {
      if (_description != value)
      {
        _description = value;
        _updated.Description = new Change<Description>(value);
      }
    }
  }
  /// <summary>
  /// The expiration date and time of the API key.
  /// </summary>
  private DateTime? _expiresOn = null;
  /// <summary>
  /// Gets or sets the expiration date and time of the API key.
  /// </summary>
  /// <exception cref="ArgumentException">The new expiration date and time was greater (more in the future) than the old expiration date and time.</exception>
  /// <exception cref="ArgumentOutOfRangeException">The date and time was not set in the future.</exception>
  public DateTime? ExpiresOn
  {
    get => _expiresOn;
    set
    {
      if (value.HasValue && value.Value.AsUniversalTime() <= DateTime.UtcNow)
      {
        throw new ArgumentOutOfRangeException(nameof(ExpiresOn), "The expiration date and time must be set in the future.");
      }
      if (_expiresOn.HasValue && (!value.HasValue || value.Value.AsUniversalTime() > _expiresOn.Value.AsUniversalTime()))
      {
        throw new ArgumentException("The API key expiration cannot be extended.", nameof(ExpiresOn));
      }

      if (_expiresOn != value)
      {
        _expiresOn = value;
        _updated.ExpiresOn = value;
      }
    }
  }

  /// <summary>
  /// Gets of sets the date and time of the latest authentication of this API key.
  /// </summary>
  public DateTime? AuthenticatedOn { get; private set; }

  /// <summary>
  /// The custom attributes of the API key.
  /// </summary>
  private readonly Dictionary<string, string> _customAttributes = [];
  /// <summary>
  /// Gets the custom attributes of the API key.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  private readonly HashSet<RoleId> _roles = [];
  /// <summary>
  /// Gets the roles of the API key.
  /// </summary>
  public IReadOnlyCollection<RoleId> Roles => _roles.ToList().AsReadOnly();

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKey"/> class.
  /// DO NOT use this constructor to create a new API key. It is only intended to be used for event sourcing.
  /// </summary>
  public ApiKey() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKey"/> class.
  /// DO use this constructor to create a new API key.
  /// </summary>
  /// <param name="displayName">The unique name of the API key.</param>
  /// <param name="secret">The secret of the API key.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="id">The identifier of the API key.</param>
  public ApiKey(DisplayName displayName, Password secret, ActorId? actorId = null, ApiKeyId? id = null) : base((id ?? ApiKeyId.NewId()).StreamId)
  {
    Raise(new ApiKeyCreated(displayName, secret), actorId);
  }
  /// <summary>
  /// Handles the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(ApiKeyCreated @event)
  {
    _secret = @event.Secret;

    _displayName = @event.DisplayName;
  }

  /// <summary>
  /// Adds the specified role to the API key, if the API key does not already have the specified role.
  /// </summary>
  /// <param name="role">The role to be added.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <exception cref="TenantMismatchException">The role and API key tenant identifiers do not match.</exception>
  public void AddRole(Role role, ActorId? actorId = null)
  {
    if (role.TenantId != TenantId)
    {
      throw new TenantMismatchException(TenantId, role.TenantId);
    }

    if (!HasRole(role))
    {
      Raise(new ApiKeyRoleAdded(role.Id), actorId);
    }
  }
  /// <summary>
  /// Handles the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(ApiKeyRoleAdded @event)
  {
    _roles.Add(@event.RoleId);
  }

  /// <summary>
  /// Authenticates the API key.
  /// </summary>
  /// <param name="secret">The current secret of the API key.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the API key's identifier.</param>
  /// <exception cref="ApiKeyIsExpiredException">The API key is expired.</exception>
  /// <exception cref="IncorrectApiKeySecretException">The secret is incorrect.</exception>
  public void Authenticate(string secret, ActorId? actorId = null)
  {
    if (IsExpired())
    {
      throw new ApiKeyIsExpiredException(this);
    }
    else if (_secret == null || !_secret.IsMatch(secret))
    {
      throw new IncorrectApiKeySecretException(this, secret);
    }

    actorId ??= new(Id.Value);
    Raise(new ApiKeyAuthenticated(), actorId.Value);
  }
  /// <summary>
  /// Handles the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(ApiKeyAuthenticated @event)
  {
    AuthenticatedOn = @event.OccurredOn;
  }

  /// <summary>
  /// Deletes the API key, if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId? actorId = null)
  {
    if (!IsDeleted)
    {
      Raise(new ApiKeyDeleted(), actorId);
    }
  }

  /// <summary>
  /// Returns a value indicating whether or not the user has the specified role.
  /// </summary>
  /// <param name="role">The role to match.</param>
  /// <returns>True if the user has the specified role, or false otherwise.</returns>
  public bool HasRole(Role role) => _roles.Contains(role.Id);

  /// <summary>
  /// Returns a value indicating whether or not the API key is expired.
  /// </summary>
  /// <param name="moment">(Optional) The date and time to verify the expiration. Defaults to now.</param>
  /// <returns>True if the API key is expired, or false otherwise.</returns>
  public bool IsExpired(DateTime? moment = null) => ExpiresOn.HasValue && ExpiresOn.Value <= (moment ?? DateTime.Now);

  /// <summary>
  /// Removes the specified role from the API key, if the API key has the specified role.
  /// </summary>
  /// <param name="role">The role to be removed.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void RemoveRole(Role role, ActorId? actorId = null)
  {
    if (HasRole(role))
    {
      Raise(new ApiKeyRoleRemoved(role.Id), actorId);
    }
  }
  /// <summary>
  /// Handles the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(ApiKeyRoleRemoved @event)
  {
    _roles.Remove(@event.RoleId);
  }

  /// <summary>
  /// Removes the specified custom attribute on the API key.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();
    if (_customAttributes.Remove(key))
    {
      _updated.CustomAttributes[key] = null;
    }
  }

  /// <summary>
  /// Sets the specified custom attribute on the API key. If the value is null, empty or only white-space, the custom attribute will be removed.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  /// <exception cref="ArgumentException">The key was not a valid identifier.</exception>
  public void SetCustomAttribute(string key, string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      RemoveCustomAttribute(key);
    }

    key = key.Trim();
    value = value.Trim();
    if (!key.IsIdentifier())
    {
      throw new ArgumentException("The value must be an identifier.", nameof(key));
    }

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || existingValue != value)
    {
      _customAttributes[key] = value;
      _updated.CustomAttributes[key] = value;
    }
  }

  /// <summary>
  /// Applies updates on the API key.
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
  /// Handles the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(ApiKeyUpdated @event)
  {
    if (@event.DisplayName != null)
    {
      _displayName = @event.DisplayName;
    }
    if (@event.Description != null)
    {
      _description = @event.Description.Value;
    }
    if (@event.ExpiresOn.HasValue)
    {
      ExpiresOn = @event.ExpiresOn.Value;
    }

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
