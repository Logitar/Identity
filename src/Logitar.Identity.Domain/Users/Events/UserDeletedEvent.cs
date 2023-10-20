using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an user is deleted.
/// </summary>
public record UserDeletedEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserDeletedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public UserDeletedEvent(ActorId actorId)
  {
    ActorId = actorId;
    IsDeleted = true;
  }
}
