using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.ApiKeys.Events;

/// <summary>
/// The event raised when an API key is deleted.
/// </summary>
public record ApiKeyDeletedEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyDeletedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public ApiKeyDeletedEvent(ActorId actorId)
  {
    ActorId = actorId;
    IsDeleted = true;
  }
}
