using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Sessions.Events;

/// <summary>
/// The event raised when a session is deleted.
/// </summary>
public record SessionDeletedEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionDeletedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public SessionDeletedEvent(ActorId actorId)
  {
    ActorId = actorId;
    IsDeleted = true;
  }
}
