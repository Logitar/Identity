using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when a role is added to an API key.
/// </summary>
public record ApiKeyRoleAdded : DomainEvent, INotification
{
  /// <summary>
  /// Gets the role identifier.
  /// </summary>
  public RoleId RoleId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyRoleAdded"/> class.
  /// </summary>
  /// <param name="roleId">The role identifier.</param>
  public ApiKeyRoleAdded(RoleId roleId)
  {
    RoleId = roleId;
  }
}
