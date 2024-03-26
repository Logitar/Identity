using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an user is deleted.
/// </summary>
public record UserDeletedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserDeletedEvent"/> class.
  /// </summary>
  public UserDeletedEvent()
  {
    IsDeleted = true;
  }
}
