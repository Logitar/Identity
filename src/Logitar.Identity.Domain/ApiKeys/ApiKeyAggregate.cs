using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.Domain.ApiKeys.Validators;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys;

public class ApiKeyAggregate : AggregateRoot
{
  private ApiKeyUpdatedEvent _updatedEvent = new();

  private Password? _secret = null;

  public new ApiKeyId Id => new(base.Id);

  public TenantId? TenantId { get; private set; }

  private DisplayNameUnit? _displayName = null;
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
  private readonly ExpirationValidator _expirationValidator = new();
  private DateTime? _expiresOn = null;
  public DateTime? ExpiresOn
  {
    get => _expiresOn;
    set
    {
      if (value != _expiresOn)
      {
        if (!value.HasValue || value.Value > _expiresOn)
        {
          throw new CannotPostponeApiKeyExpirationException(this, value);
        }
        _expirationValidator.ValidateAndThrow(value.Value);

        _expiresOn = value;
        _updatedEvent.ExpiresOn = value;
      }
    }
  }

  public ApiKeyAggregate(AggregateId id) : base(id)
  {
  }

  public ApiKeyAggregate(Password secret, DisplayNameUnit displayName, TenantId? tenantId = null, ActorId actorId = default, ApiKeyId? id = null)
    : base((id ?? ApiKeyId.NewId()).AggregateId)
  {
    Raise(new ApiKeyCreatedEvent(actorId, displayName, secret, tenantId));
  }
  protected virtual void Apply(ApiKeyCreatedEvent @event)
  {
    _secret = @event.Secret;

    TenantId = @event.TenantId;

    _displayName = @event.DisplayName;
  }

  public DateTime? AuthenticatedOn { get; private set; }

  private readonly Dictionary<string, string> _customAttributes = [];
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  private readonly HashSet<RoleId> _roles = [];
  public IReadOnlyCollection<RoleId> Roles => _roles.ToList().AsReadOnly();

  public void AddRole(RoleAggregate role, ActorId actorId = default)
  {
    if (!HasRole(role))
    {
      Raise(new ApiKeyRoleAddedEvent(actorId, role.Id));
    }
  }
  protected virtual void Apply(ApiKeyRoleAddedEvent @event)
  {
    _roles.Add(@event.RoleId);
  }

  public void Authenticate(byte[] secret, ActorId actorId = default)
  {
    if (_secret == null || !_secret.IsMatch(secret))
    {
      throw new IncorrectApiKeySecretException(this, secret);
    }
    else if (IsExpired())
    {
      throw new ApiKeyIsExpiredException(this);
    }

    Raise(new ApiKeyAuthenticatedEvent(actorId));
  }
  protected virtual void Apply(ApiKeyAuthenticatedEvent @event)
  {
    AuthenticatedOn = @event.OccurredOn;
  }

  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new ApiKeyDeletedEvent(actorId));
    }
  }

  public bool HasRole(RoleAggregate role) => HasRole(role.Id);
  public bool HasRole(RoleId id) => _roles.Contains(id);

  public bool IsExpired() => IsExpired(moment: null);
  public bool IsExpired(DateTime? moment) => _expiresOn.HasValue && ((moment ?? DateTime.Now) >= _expiresOn.Value);

  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();

    if (_customAttributes.ContainsKey(key))
    {
      _updatedEvent.CustomAttributes[key] = null;
      _customAttributes.Remove(key);
    }
  }

  public void RemoveRole(RoleAggregate role, ActorId actorId = default)
  {
    if (HasRole(role))
    {
      Raise(new ApiKeyRoleRemovedEvent(actorId, role.Id));
    }
  }
  protected virtual void Apply(ApiKeyRoleRemovedEvent @event)
  {
    _roles.Remove(@event.RoleId);
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

  public void Update(ActorId actorId = default)
  {
    if (_updatedEvent.HasChanges)
    {
      _updatedEvent.ActorId = actorId;
      Raise(_updatedEvent);
      _updatedEvent = new();
    }
  }
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
      _expiresOn = @event.ExpiresOn.Value;
    }

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

  public override string ToString() => $"{DisplayName.Value} | {base.ToString()}";
}
