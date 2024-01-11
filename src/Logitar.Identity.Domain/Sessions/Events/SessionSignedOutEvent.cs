using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

/// <summary>
/// The event raised when an active session is signed-out.
/// </summary>
public record SessionSignedOutEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionSignedOutEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public SessionSignedOutEvent(ActorId actorId)
  {
    ActorId = actorId;
  }
}
