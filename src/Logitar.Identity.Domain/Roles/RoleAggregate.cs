using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Roles;

public class RoleAggregate : AggregateRoot
{
  private RoleUpdatedEvent _updatedEvent = new();

  public new RoleId Id => new(base.Id);

  public TenantId? TenantId { get; private set; }

  private UniqueNameUnit? _uniqueName = null;
  public UniqueNameUnit UniqueName => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");

  public DisplayNameUnit? _displayName = null;
  public DisplayNameUnit? DisplayName
  {
    get => _displayName;
    set
    {
      if (value != _displayName)
      {
        _displayName = value;
        _updatedEvent.DisplayName = new Modification<DisplayNameUnit>(value);
      }
    }
  }
  public DescriptionUnit? _description = null;
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

  private readonly Dictionary<string, string> _customAttributes = [];
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  public RoleAggregate(AggregateId id) : base(id)
  {
  }

  public RoleAggregate(UniqueNameUnit uniqueName, TenantId? tenantId = null, ActorId actorId = default, RoleId? id = null)
    : base((id ?? RoleId.NewId()).AggregateId)
  {
    Raise(new RoleCreatedEvent(actorId, tenantId, uniqueName));
  }
  protected virtual void Apply(RoleCreatedEvent @event)
  {
    TenantId = @event.TenantId;

    _uniqueName = @event.UniqueName;
  }

  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new RoleDeletedEvent(actorId));
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

  public void SetUniqueName(UniqueNameUnit uniqueName, ActorId actorId = default)
  {
    if (uniqueName != _uniqueName)
    {
      Raise(new RoleUniqueNameChangedEvent(actorId, uniqueName));
    }
  }
  protected virtual void Apply(RoleUniqueNameChangedEvent @event)
  {
    _uniqueName = @event.UniqueName;
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
  protected virtual void Apply(RoleUpdatedEvent @event)
  {
    if (@event.DisplayName != null)
    {
      _displayName = @event.DisplayName.Value;
    }
    if (@event.Description != null)
    {
      _description = @event.Description.Value;
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

  public override string ToString() => $"{DisplayName?.Value ?? UniqueName.Value} | {base.ToString()}";
}
