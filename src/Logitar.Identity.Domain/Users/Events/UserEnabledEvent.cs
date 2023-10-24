using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an user is enabled.
/// </summary>
public record UserEnabledEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserEnabledEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public UserEnabledEvent(ActorId actorId)
  {
    ActorId = actorId;
  }
}
