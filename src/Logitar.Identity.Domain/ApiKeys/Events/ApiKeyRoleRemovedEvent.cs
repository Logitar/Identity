using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

/// <summary>
/// The event raised when a role is removed from an API key.
/// </summary>
public record ApiKeyRoleRemovedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the role identifier.
  /// </summary>
  public RoleId RoleId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyRoleRemovedEvent"/> class.
  /// </summary>
  /// <param name="roleId">The role identifier.</param>
  public ApiKeyRoleRemovedEvent(RoleId roleId)
  {
    RoleId = roleId;
  }
}
