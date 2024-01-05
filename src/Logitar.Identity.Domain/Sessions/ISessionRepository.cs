using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Sessions;

public interface ISessionRepository
{
  Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken = default);
  Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, bool includeDeleted, CancellationToken cancellationToken = default);

  Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<SessionAggregate> sessions, CancellationToken cancellationToken = default);
}
