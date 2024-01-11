using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.ApiKeys.Events;

public record ApiKeyAuthenticatedEvent : DomainEvent
{
  public ApiKeyAuthenticatedEvent(ActorId actorId)
  {
    ActorId = actorId;
  }
}
