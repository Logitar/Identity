using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an user signs-in.
/// </summary>
public class UserSignedInEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserSignedInEvent"/> class.
  /// </summary>
  /// <param name="occurredOn">The date and time when the authentication occurred.</param>
  public UserSignedInEvent(DateTime occurredOn)
  {
    OccurredOn = occurredOn;
  }
}
