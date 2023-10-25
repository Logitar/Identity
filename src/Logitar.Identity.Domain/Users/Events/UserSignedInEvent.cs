using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an user signs-in.
/// </summary>
public record UserSignedInEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserSignedInEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="occurredOn">The date and time when the authentication occurred.</param>
  public UserSignedInEvent(ActorId actorId, DateTime occurredOn)
  {
    ActorId = actorId;
    OccurredOn = occurredOn;
  }
}
