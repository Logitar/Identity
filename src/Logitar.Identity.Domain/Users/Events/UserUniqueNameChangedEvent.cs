using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users.Events;

public record UserUniqueNameChangedEvent : DomainEvent
{
  public UniqueNameUnit UniqueName { get; }

  public UserUniqueNameChangedEvent(ActorId actorId, UniqueNameUnit uniqueName)
  {
    ActorId = actorId;
    UniqueName = uniqueName;
  }
}
