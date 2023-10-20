using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when a role is added to an user.
/// </summary>
public record UserRoleAddedEvent : DomainEvent
{
  /// <summary>
  /// Gets the role identifier.
  /// </summary>
  public RoleId RoleId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserRoleAddedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="roleId">The role identifier.</param>
  public UserRoleAddedEvent(ActorId actorId, RoleId roleId)
  {
    ActorId = actorId;
    RoleId = roleId;
  }
}
