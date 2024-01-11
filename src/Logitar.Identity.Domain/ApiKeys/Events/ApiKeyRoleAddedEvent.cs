using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Domain.ApiKeys.Events;

/// <summary>
/// The event raised when a role is added to an API key.
/// </summary>
public record ApiKeyRoleAddedEvent : DomainEvent
{
  /// <summary>
  /// Gets the role identifier.
  /// </summary>
  public RoleId RoleId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyRoleAddedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="roleId">The role identifier.</param>
  public ApiKeyRoleAddedEvent(ActorId actorId, RoleId roleId)
  {
    ActorId = actorId;
    RoleId = roleId;
  }
}
