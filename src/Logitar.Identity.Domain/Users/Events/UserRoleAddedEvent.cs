using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Domain.Users.Events;

public record UserRoleAddedEvent : DomainEvent
{
  public RoleId RoleId { get; }

  public UserRoleAddedEvent(ActorId actorId, RoleId roleId)
  {
    ActorId = actorId;
    RoleId = roleId;
  }
}
