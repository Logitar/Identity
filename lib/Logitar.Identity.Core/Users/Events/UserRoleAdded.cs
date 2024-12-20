using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a role is added to an user.
/// </summary>
public record UserRoleAdded : DomainEvent, INotification
{
  /// <summary>
  /// Gets the role identifier.
  /// </summary>
  public RoleId RoleId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserRoleAdded"/> class.
  /// </summary>
  /// <param name="roleId">The role identifier.</param>
  public UserRoleAdded(RoleId roleId)
  {
    RoleId = roleId;
  }
}
