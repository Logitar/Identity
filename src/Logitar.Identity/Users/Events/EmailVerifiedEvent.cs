using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Users.Events;

/// <summary>
/// Represents the event raised when the email address of an <see cref="UserAggregate"/> is updated.
/// </summary>
public record EmailUpdatedEvent : DomainEvent, INotification, ISetEmail
{
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public ReadOnlyEmail? Email { get; init; }
  /// <summary>
  /// Gets or sets the email address verification action performed by the event.
  /// </summary>
  public VerificationAction EmailVerification { get; init; }
}
