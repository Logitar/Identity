using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users;

public interface IUserManager
{
  Task SaveAsync(UserAggregate user, ActorId actorId = default, CancellationToken cancellationToken = default);
}
