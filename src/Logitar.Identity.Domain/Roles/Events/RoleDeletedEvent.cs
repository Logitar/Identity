using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Roles.Events;

/// <summary>
/// The event raised when a role is deleted.
/// </summary>
public record RoleDeletedEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RoleDeletedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public RoleDeletedEvent(ActorId actorId)
  {
    ActorId = actorId;
    IsDeleted = true;
  }
}
