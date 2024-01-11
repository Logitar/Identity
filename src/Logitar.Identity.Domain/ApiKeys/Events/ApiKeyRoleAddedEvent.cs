using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Domain.ApiKeys.Events;

public record ApiKeyRoleAddedEvent : DomainEvent
{
  public RoleId RoleId { get; }

  public ApiKeyRoleAddedEvent(ActorId actorId, RoleId roleId)
  {
    ActorId = actorId;
    RoleId = roleId;
  }
}
