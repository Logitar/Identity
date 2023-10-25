using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Roles;

/// <summary>
/// Represents a role in the identity system. A role is a group of permissions.
/// A role can be assigned to any actor or actor group, and an actor or actor group can have more than one role.
/// </summary>
public class RoleAggregate : AggregateRoot
{
  private readonly Dictionary<string, string> _customAttributes = new();
  private RoleUpdatedEvent _updated = new();

  /// <summary>
  /// Gets the identifier of the role.
  /// </summary>
  public new RoleId Id => new(base.Id);

  /// <summary>
  /// Gets the tenant identifier of the role.
  /// </summary>
  public TenantId? TenantId { get; private set; }

  /// <summary>
  /// The unique name of the role.
  /// </summary>
  private UniqueNameUnit? _uniqueName = null;
  /// <summary>
  /// Gets the unique name of the role.
  /// </summary>
  public UniqueNameUnit UniqueName => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");

  private DisplayNameUnit? _displayName = null;
  /// <summary>
  /// Gets or sets the display name of the role.
  /// </summary>
  public DisplayNameUnit? DisplayName
  {
    get => _displayName;
    set
    {
      if (value != _displayName)
      {
        _updated.DisplayName = new Modification<DisplayNameUnit>(value);
        _displayName = value;
      }
    }
  }

  private DescriptionUnit? _description = null;
  /// <summary>
  /// Gets or sets the description of the role.
  /// </summary>
  public DescriptionUnit? Description
  {
    get => _description;
    set
    {
      if (value != _description)
      {
        _updated.Description = new Modification<DescriptionUnit>(value);
        _description = value;
      }
    }
  }

  /// <summary>
  /// Gets the custom attributes of the role.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleAggregate"/> class.
  /// DO NOT use this constructor to create a new role. It is only intended to be used by the event sourcing.
  /// </summary>
  /// <param name="id">The identifier of the role.</param>
  public RoleAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleAggregate"/> class.
  /// DO use this constructor to create a new role.
  /// </summary>
  /// <param name="uniqueName">The unique name of the role.</param>
  /// <param name="tenantId">The tenant identifier of the role.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="id">The identifier of the role.</param>
  public RoleAggregate(UniqueNameUnit uniqueName, TenantId? tenantId = null, ActorId actorId = default, RoleId? id = null)
    : base(id?.AggregateId)
  {
    ApplyChange(new RoleCreatedEvent(actorId, uniqueName, tenantId));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(RoleCreatedEvent @event)
  {
    TenantId = @event.TenantId;

    _uniqueName = @event.UniqueName;
  }

  /// <summary>
  /// Deletes the role, if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      ApplyChange(new RoleDeletedEvent(actorId));
    }
  }

  /// <summary>
  /// Removes the specified custom attribute on the role.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();

    if (_customAttributes.ContainsKey(key))
    {
      _updated.CustomAttributes[key] = null;
      _customAttributes.Remove(key);
    }
  }

  private readonly CustomAttributeValidator _customAttributeValidator = new();
  /// <summary>
  /// Sets the specified custom attribute on the role.
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
      _updated.CustomAttributes[key] = value;
      _customAttributes[key] = value;
    }
  }

  /// <summary>
  /// Sets the unique name of the role.
  /// </summary>
  /// <param name="uniqueName">The unique name.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetUniqueName(UniqueNameUnit uniqueName, ActorId actorId = default)
  {
    if (uniqueName != _uniqueName)
    {
      ApplyChange(new RoleUniqueNameChangedEvent(actorId, uniqueName));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(RoleUniqueNameChangedEvent @event) => _uniqueName = @event.UniqueName;

  /// <summary>
  /// Applies updates on the role.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Update(ActorId actorId = default)
  {
    if (_updated.HasChanges)
    {
      _updated.ActorId = actorId;

      ApplyChange(_updated);

      _updated = new();
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
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
  /// Returns a string representation of the role.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{DisplayName?.Value ?? UniqueName.Value} | {base.ToString()}";
}
