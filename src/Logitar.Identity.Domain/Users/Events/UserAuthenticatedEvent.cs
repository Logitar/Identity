using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an user is authenticated.
/// </summary>
public record UserAuthenticatedEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserAuthenticatedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public UserAuthenticatedEvent(ActorId actorId)
  {
    ActorId = actorId;
  }
}
