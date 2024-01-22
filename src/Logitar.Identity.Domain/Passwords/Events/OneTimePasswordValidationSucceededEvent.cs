using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Passwords.Events;

/// <summary>
/// The event raised when a One-Time Password (OTP) is successfully validated.
/// </summary>
public record OneTimePasswordValidationSucceededEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordValidationSucceededEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public OneTimePasswordValidationSucceededEvent(ActorId actorId)
  {
    ActorId = actorId;
  }
}
