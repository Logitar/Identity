using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a role is removed from an user.
/// </summary>
public record UserRoleRemoved : DomainEvent, INotification
{
  /// <summary>
  /// Gets the role identifier.
  /// </summary>
  public RoleId RoleId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserRoleRemoved"/> class.
  /// </summary>
  /// <param name="roleId">The role identifier.</param>
  public UserRoleRemoved(RoleId roleId)
  {
    RoleId = roleId;
  }
}
