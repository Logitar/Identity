using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

/// <summary>
/// The event raised when a session is deleted.
/// </summary>
public class SessionDeletedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionDeletedEvent"/> class.
  /// </summary>
  public SessionDeletedEvent()
  {
    IsDeleted = true;
  }
}
