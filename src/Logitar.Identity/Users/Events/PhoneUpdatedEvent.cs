using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Users.Events;

/// <summary>
/// Represents the event raised when the phone number of an <see cref="UserAggregate"/> is updated.
/// </summary>
public record PhoneUpdatedEvent : DomainEvent, INotification, ISetPhone
{
  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public ReadOnlyPhone? Phone { get; init; }
  /// <summary>
  /// Gets or sets the phone number verification action performed by the event.
  /// </summary>
  public VerificationAction PhoneVerification { get; init; }
}
