using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

/// <summary>
/// The event raised when an API key is deleted.
/// </summary>
public record ApiKeyDeletedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyDeletedEvent"/> class.
  /// </summary>
  public ApiKeyDeletedEvent()
  {
    IsDeleted = true;
  }
}
