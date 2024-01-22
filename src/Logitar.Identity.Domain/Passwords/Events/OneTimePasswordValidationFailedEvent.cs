using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Passwords.Events;

/// <summary>
/// The event raised when a One-Time Password (OTP) validation failed.
/// </summary>
public record OneTimePasswordValidationFailedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordValidationFailedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public OneTimePasswordValidationFailedEvent(ActorId actorId)
  {
    ActorId = actorId;
  }
}
