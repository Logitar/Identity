using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Passwords.Events;

/// <summary>
/// The event raised when a One-Time Password (OTP) is deleted.
/// </summary>
public class OneTimePasswordDeletedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordDeletedEvent"/> class.
  /// </summary>
  public OneTimePasswordDeletedEvent()
  {
    IsDeleted = true;
  }
}
