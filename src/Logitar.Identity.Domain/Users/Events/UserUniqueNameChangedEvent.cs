using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserUniqueNameChangedEvent : DomainEvent, INotification
{
  public UniqueNameUnit UniqueName { get; }

  public UserUniqueNameChangedEvent(ActorId actorId, UniqueNameUnit uniqueName)
  {
    ActorId = actorId;
    UniqueName = uniqueName;
  }
}
