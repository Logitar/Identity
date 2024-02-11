using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Shared.Validators;

namespace Logitar.Identity.Domain.ApiKeys;

/// <summary>
/// Represents an API key in the identity system. Similarly to an user, it can have access to processes and resources using roles.
/// It should be used instead of users for backend-to-backend authentication where personal and authentication information are not required.
/// </summary>
public class ApiKeyAggregate : AggregateRoot
{
  private ApiKeyUpdatedEvent _updatedEvent = new();

  private Password? _secret = null;

  /// <summary>
  /// Gets the identifier of the API key.
  /// </summary>
  public new ApiKeyId Id => new(base.Id);

  /// <summary>
  /// Gets the tenant identifier of the API key.
  /// </summary>
  public TenantId? TenantId { get; private set; }

  private DisplayNameUnit? _displayName = null;
  /// <summary>
  /// Gets or sets the display name of the API key.
  /// </summary>
  /// <exception cref="InvalidOperationException">The display name has not been initialized yet.</exception>
  public DisplayNameUnit DisplayName
  {
    get => _displayName ?? throw new InvalidOperationException($"The {nameof(DisplayName)} has not been initialized yet.");
    set
    {
      if (value != _displayName)
      {
        _displayName = value;
        _updatedEvent.DisplayName = value;
      }
    }
  }
  private DescriptionUnit? _description = null;
  /// <summary>
  /// Gets or sets the description of the API key.
  /// </summary>
  public DescriptionUnit? Description
  {
    get => _description;
    set
    {
      if (value != _description)
      {
        _description = value;
        _updatedEvent.Description = new Modification<DescriptionUnit>(value);
      }
    }
  }
  /// <summary>
  /// Gets or sets the expiration date and time of the API key.
  /// </summary>
  public DateTime? ExpiresOn { get; private set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyAggregate"/> class.
  /// DO NOT use this constructor to create a new API key. It is only intended to be used by the event sourcing.
  /// </summary>
  /// <param name="id">The identifier of the API key.</param>
  public ApiKeyAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyAggregate"/> class.
  /// DO use this constructor to create a new API key.
  /// </summary>
  /// <param name="displayName">The unique name of the API key.</param>
  /// <param name="secret">The secret of the API key.</param>
  /// <param name="tenantId">The tenant identifier of the API key.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="id">The identifier of the API key.</param>
  public ApiKeyAggregate(DisplayNameUnit displayName, Password secret, TenantId? tenantId = null, ActorId actorId = default, ApiKeyId? id = null)
    : base((id ?? ApiKeyId.NewId()).AggregateId)
  {
    Raise(new ApiKeyCreatedEvent(actorId, displayName, secret, tenantId));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(ApiKeyCreatedEvent @event)
  {
    _secret = @event.Secret;

    TenantId = @event.TenantId;

    _displayName = @event.DisplayName;
  }

  /// <summary>
  /// Gets of sets the date and time of the latest authentication of this API key.
  /// </summary>
  public DateTime? AuthenticatedOn { get; private set; }

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
  /// Adds the specified role to the API key, if the API key does not already have the specified role.
  /// </summary>
  /// <param name="role">The role to be added.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <exception cref="TenantMismatchException">The role and API key tenant identifiers do not match.</exception>
  public void AddRole(RoleAggregate role, ActorId actorId = default)
  {
    if (role.TenantId != TenantId)
    {
      throw new TenantMismatchException(TenantId, role.TenantId);
    }

    if (!HasRole(role))
    {
      Raise(new ApiKeyRoleAddedEvent(actorId, role.Id));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(ApiKeyRoleAddedEvent @event)
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
    Raise(new ApiKeyAuthenticatedEvent(actorId.Value));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(ApiKeyAuthenticatedEvent @event)
  {
    AuthenticatedOn = @event.OccurredOn;
  }

  /// <summary>
  /// Deletes the API key, if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new ApiKeyDeletedEvent(actorId));
    }
  }

  /// <summary>
  /// Returns a value indicating whether or not the user has the specified role.
  /// </summary>
  /// <param name="role">The role to match.</param>
  /// <returns>True if the user has the specified role, or false otherwise.</returns>
  public bool HasRole(RoleAggregate role) => _roles.Contains(role.Id);

  /// <summary>
  /// Returns a value indicating whether or not the API key is expired.
  /// </summary>
  /// <param name="moment">(Optional) The date and time to verify the expiration. Defaults to now.</param>
  /// <returns>True if the API key is expired, or false otherwise.</returns>
  public bool IsExpired(DateTime? moment = null) => ExpiresOn.HasValue && ExpiresOn.Value <= (moment ?? DateTime.Now);

  /// <summary>
  /// Removes the specified custom attribute on the API key.
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
  /// Removes the specified role from the API key, if the API key has the specified role.
  /// </summary>
  /// <param name="role">The role to be removed.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void RemoveRole(RoleAggregate role, ActorId actorId = default)
  {
    if (HasRole(role))
    {
      Raise(new ApiKeyRoleRemovedEvent(actorId, role.Id));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(ApiKeyRoleRemovedEvent @event)
  {
    _roles.Remove(@event.RoleId);
  }

  private readonly CustomAttributeValidator _customAttributeValidator = new();
  /// <summary>
  /// Sets the specified custom attribute on the API key.
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
  /// Sets the expiration of the API key.
  /// </summary>
  /// <param name="expiresOn">The expiration date and time.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public void SetExpiration(DateTime expiresOn, string? propertyName = null)
  {
    if (expiresOn != ExpiresOn)
    {
      new ExpirationValidator(ExpiresOn, propertyName).ValidateAndThrow(expiresOn);

      ExpiresOn = expiresOn;
      _updatedEvent.ExpiresOn = expiresOn;
    }
  }

  /// <summary>
  /// Applies updates on the API key.
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
  protected virtual void Apply(ApiKeyUpdatedEvent @event)
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

  /// <summary>
  /// Returns a string representation of the API key.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{DisplayName.Value} | {base.ToString()}";
}
