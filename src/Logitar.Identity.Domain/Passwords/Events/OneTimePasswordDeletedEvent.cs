using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Passwords.Events;

/// <summary>
/// The event raised when a One-Time Password (OTP) is deleted.
/// </summary>
public record OneTimePasswordDeletedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordDeletedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public OneTimePasswordDeletedEvent(ActorId actorId)
  {
    ActorId = actorId;
    IsDeleted = true;
  }
}
