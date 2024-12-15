using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when a role is removed from an API key.
/// </summary>
public record ApiKeyRoleRemoved : DomainEvent, INotification
{
  /// <summary>
  /// Gets the role identifier.
  /// </summary>
  public RoleId RoleId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyRoleRemoved"/> class.
  /// </summary>
  /// <param name="roleId">The role identifier.</param>
  public ApiKeyRoleRemoved(RoleId roleId)
  {
    RoleId = roleId;
  }
}
