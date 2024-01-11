using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an user is disabled.
/// </summary>
public record UserDisabledEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserDisabledEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public UserDisabledEvent(ActorId actorId)
  {
    ActorId = actorId;
  }
}
