using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles.Events;

namespace Logitar.Identity.Core.Roles;

/// <summary>
/// Represents a role in the identity system. A role is a group of permissions.
/// A role can be assigned to any actor or actor group, and an actor or actor group can have more than one role.
/// </summary>
public class Role : AggregateRoot
{
  /// <summary>
  /// The updated event.
  /// </summary>
  private RoleUpdated _updated = new();

  /// <summary>
  /// Gets the identifier of the role.
  /// </summary>
  public new RoleId Id => new(base.Id);
  /// <summary>
  /// Gets the tenant identifier of the role.
  /// </summary>
  public TenantId? TenantId => Id.TenantId;
  /// <summary>
  /// Gets the entity identifier of the role. This identifier is unique within the tenant.
  /// </summary>
  public EntityId EntityId => Id.EntityId;

  /// <summary>
  /// The unique name of the role.
  /// </summary>
  private UniqueName? _uniqueName = null;
  /// <summary>
  /// Gets the unique name of the role.
  /// </summary>
  /// <exception cref="InvalidOperationException">The unique name has not been initialized yet.</exception>
  public UniqueName UniqueName => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");
  /// <summary>
  /// The display name of the role.
  /// </summary>
  private DisplayName? _displayName = null;
  /// <summary>
  /// Gets or sets the display name of the role.
  /// </summary>
  public DisplayName? DisplayName
  {
    get => _displayName;
    set
    {
      if (_displayName != value)
      {
        _displayName = value;
        _updated.DisplayName = new Change<DisplayName>(value);
      }
    }
  }
  /// <summary>
  /// The description of the role.
  /// </summary>
  private Description? _description = null;
  /// <summary>
  /// Gets or sets the description of the role.
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
  /// The custom attributes of the role.
  /// </summary>
  private readonly Dictionary<Identifier, string> _customAttributes = [];
  /// <summary>
  /// Gets the custom attributes of the role.
  /// </summary>
  public IReadOnlyDictionary<Identifier, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Initializes a new instance of the <see cref="Role"/> class.
  /// DO NOT use this constructor to create a new role. It is only intended to be used for event sourcing.
  /// </summary>
  public Role() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Role"/> class.
  /// DO use this constructor to create a new role.
  /// </summary>
  /// <param name="uniqueName">The unique name of the role.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="roleId">The role identifier.</param>
  public Role(UniqueName uniqueName, ActorId? actorId = null, RoleId? roleId = null) : base((roleId ?? RoleId.NewId()).StreamId)
  {
    Raise(new RoleCreated(uniqueName), actorId);
  }
  /// <summary>
  /// Handles the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(RoleCreated @event)
  {
    _uniqueName = @event.UniqueName;
  }

  /// <summary>
  /// Deletes the role, if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId? actorId = null)
  {
    if (!IsDeleted)
    {
      Raise(new RoleDeleted(), actorId);
    }
  }

  /// <summary>
  /// Removes the specified custom attribute on the role.
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
  /// Sets the specified custom attribute on the role. If the value is null, empty or only white-space, the custom attribute will be removed.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  public void SetCustomAttribute(Identifier key, string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      RemoveCustomAttribute(key);
    }
    else
    {
      value = value.Trim();

      if (!_customAttributes.TryGetValue(key, out string? existingValue) || existingValue != value)
      {
        _customAttributes[key] = value;
        _updated.CustomAttributes[key] = value;
      }
    }
  }

  /// <summary>
  /// Sets the unique name of the role.
  /// </summary>
  /// <param name="uniqueName">The unique name.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetUniqueName(UniqueName uniqueName, ActorId? actorId = null)
  {
    if (_uniqueName != uniqueName)
    {
      Raise(new RoleUniqueNameChanged(uniqueName), actorId);
    }
  }
  /// <summary>
  /// Handles the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(RoleUniqueNameChanged @event)
  {
    _uniqueName = @event.UniqueName;
  }

  /// <summary>
  /// Applies updates on the role.
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
  protected virtual void Handle(RoleUpdated @event)
  {
    if (@event.DisplayName != null)
    {
      _displayName = @event.DisplayName.Value;
    }
    if (@event.Description != null)
    {
      _description = @event.Description.Value;
    }

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

  /// <summary>
  /// Returns a string representation of the role.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{DisplayName?.Value ?? UniqueName.Value} | {base.ToString()}";
}
