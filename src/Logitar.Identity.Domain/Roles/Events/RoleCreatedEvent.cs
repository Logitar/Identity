using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Roles.Events;

public record RoleCreatedEvent : DomainEvent
{
  public TenantId? TenantId { get; }

  public UniqueNameUnit UniqueName { get; }

  public RoleCreatedEvent(ActorId actorId, TenantId? tenantId, UniqueNameUnit uniqueName)
  {
    ActorId = actorId;
    TenantId = tenantId;
    UniqueName = uniqueName;
  }
}
