using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserCreatedEvent : DomainEvent, INotification
{
  public TenantId? TenantId { get; }

  public UniqueNameUnit UniqueName { get; }

  public UserCreatedEvent(ActorId actorId, TenantId? tenantId, UniqueNameUnit uniqueName)
  {
    ActorId = actorId;
    TenantId = tenantId;
    UniqueName = uniqueName;
  }
}
