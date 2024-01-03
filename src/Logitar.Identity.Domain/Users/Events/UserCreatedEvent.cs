using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users.Events;

public record UserCreatedEvent : DomainEvent
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
