using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users;

public interface IUserManager
{
  Task<UserAggregate?> FindAsync(string? tenantId, string uniqueNameOrEmailAddress, CancellationToken cancellationToken = default);

  Task SaveAsync(UserAggregate user, ActorId actorId = default, CancellationToken cancellationToken = default);
}
