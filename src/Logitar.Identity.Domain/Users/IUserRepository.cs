using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

public interface IUserRepository
{
  Task<UserAggregate?> LoadAsync(UserId id, CancellationToken cancellationToken = default);
  Task<UserAggregate?> LoadAsync(UserId id, long? version, CancellationToken cancellationToken = default);
  Task<UserAggregate?> LoadAsync(UserId id, bool includeDeleted, CancellationToken cancellationToken = default);
  Task<UserAggregate?> LoadAsync(UserId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<UserAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<UserAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<UserAggregate>> LoadAsync(IEnumerable<UserId> ids, CancellationToken cancellationToken = default);
  Task<IEnumerable<UserAggregate>> LoadAsync(IEnumerable<UserId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<UserAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);
  Task<IEnumerable<UserAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<UserAggregate?> LoadAsync(TenantId? tenantId, UniqueNameUnit uniqueName, CancellationToken cancellationToken = default);
  Task<IEnumerable<UserAggregate>> LoadAsync(TenantId? tenantId, EmailUnit email, CancellationToken cancellationToken = default);

  Task SaveAsync(UserAggregate user, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken = default);
}
