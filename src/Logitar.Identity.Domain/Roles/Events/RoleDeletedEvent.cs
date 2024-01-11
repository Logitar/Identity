using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Roles.Events;

public record RoleDeletedEvent : DomainEvent
{
  public RoleDeletedEvent(ActorId actorId)
  {
    ActorId = actorId;
    IsDeleted = true;
  }
}
