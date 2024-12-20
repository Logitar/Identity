using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when an user signs-in.
/// </summary>
public record UserSignedIn : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserSignedIn"/> class.
  /// </summary>
  /// <param name="occurredOn">The date and time when the authentication occurred.</param>
  public UserSignedIn(DateTime occurredOn) // TODO(fpion): remove?
  {
    OccurredOn = occurredOn;
  }
}
