using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Domain.Users.Events;

public record UserRoleRemovedEvent : DomainEvent
{
  public RoleId RoleId { get; }

  public UserRoleRemovedEvent(ActorId actorId, RoleId roleId)
  {
    ActorId = actorId;
    RoleId = roleId;
  }
}
