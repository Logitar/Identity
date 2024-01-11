using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.ApiKeys.Events;

public record ApiKeyDeletedEvent : DomainEvent
{
  public ApiKeyDeletedEvent(ActorId actorId)
  {
    ActorId = actorId;
    IsDeleted = true;
  }
}
