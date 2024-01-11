using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

/// <summary>
/// The event raised when an API key is authenticated.
/// </summary>
public record ApiKeyAuthenticatedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyAuthenticatedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public ApiKeyAuthenticatedEvent(ActorId actorId)
  {
    ActorId = actorId;
  }
}
