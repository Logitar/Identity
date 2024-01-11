using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Roles;

public interface IRoleManager
{
  Task SaveAsync(RoleAggregate role, ActorId actorId = default, CancellationToken cancellationToken = default);
}
