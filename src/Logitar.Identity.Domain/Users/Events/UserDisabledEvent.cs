using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

public record UserDisabledEvent : DomainEvent
{
  public UserDisabledEvent(ActorId actorId)
  {
    ActorId = actorId;
  }
}
