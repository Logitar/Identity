using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles.Events;

namespace Logitar.Identity.Core.Roles;

/// <summary>
/// Represents a role in the identity system. A role is a group of permissions.
/// A role can be assigned to any actor or actor group, and an actor or actor group can have more than one role.
/// </summary>
public class Role : AggregateRoot
{
  // TODO(fpion): TenantId
  // TODO(fpion): CustomAttributes

  /// <summary>
  /// The updated event.
  /// </summary>
  private RoleUpdated _updated = new();

  /// <summary>
  /// Gets the identifier of the role.
  /// </summary>
  public new RoleId Id => new(base.Id);

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
  /// Initializes a new instance of the <see cref="Role"/> class.
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
    if (@event.UniqueName != null)
    {
      _uniqueName = @event.UniqueName;
    }
    if (@event.DisplayName != null)
    {
      _displayName = @event.DisplayName.Value;
    }
    if (@event.Description != null)
    {
      _description = @event.Description.Value;
    }
  }

  /// <summary>
  /// Returns a string representation of the role.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{DisplayName?.Value ?? UniqueName.Value} | {base.ToString()}";
}
