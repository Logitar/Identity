using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when a role is removed from an user.
/// </summary>
public record UserRoleRemovedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the role identifier.
  /// </summary>
  public RoleId RoleId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserRoleRemovedEvent"/> class.
  /// </summary>
  /// <param name="roleId">The role identifier.</param>
  public UserRoleRemovedEvent(RoleId roleId)
  {
    RoleId = roleId;
  }
}
