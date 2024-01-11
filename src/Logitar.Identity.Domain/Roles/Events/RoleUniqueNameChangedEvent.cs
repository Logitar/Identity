using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Roles.Events;

public record RoleUniqueNameChangedEvent : DomainEvent, INotification
{
  public UniqueNameUnit UniqueName { get; }

  public RoleUniqueNameChangedEvent(ActorId actorId, UniqueNameUnit uniqueName)
  {
    ActorId = actorId;
    UniqueName = uniqueName;
  }
}
