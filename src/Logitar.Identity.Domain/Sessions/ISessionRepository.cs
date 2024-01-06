using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Sessions;

public interface ISessionRepository
{
  Task<SessionAggregate?> LoadAsync(SessionId id, CancellationToken cancellationToken = default);
  Task<SessionAggregate?> LoadAsync(SessionId id, long? version, CancellationToken cancellationToken = default);
  Task<SessionAggregate?> LoadAsync(SessionId id, bool includeDeleted, CancellationToken cancellationToken = default);
  Task<SessionAggregate?> LoadAsync(SessionId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<SessionAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<SessionAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<SessionAggregate>> LoadAsync(IEnumerable<SessionId> ids, CancellationToken cancellationToken = default);
  Task<IEnumerable<SessionAggregate>> LoadAsync(IEnumerable<SessionId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<SessionAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);
  Task<IEnumerable<SessionAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken = default);
  Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, bool includeDeleted, CancellationToken cancellationToken = default);

  Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<SessionAggregate> sessions, CancellationToken cancellationToken = default);
}
