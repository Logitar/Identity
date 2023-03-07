using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Sessions.Events;

/// <summary>
/// Represents the event raised when an <see cref="SessionAggregate"/> is deleted.
/// </summary>
public record SessionDeletedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionDeletedEvent"/> clss.
  /// </summary>
  public SessionDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
