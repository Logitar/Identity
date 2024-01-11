using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Roles;

public interface IRoleRepository
{
  Task<RoleAggregate?> LoadAsync(RoleId id, CancellationToken cancellationToken = default);
  Task<RoleAggregate?> LoadAsync(RoleId id, long? version, CancellationToken cancellationToken = default);
  Task<RoleAggregate?> LoadAsync(RoleId id, bool includeDeleted, CancellationToken cancellationToken = default);
  Task<RoleAggregate?> LoadAsync(RoleId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<RoleAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<RoleAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<RoleAggregate>> LoadAsync(IEnumerable<RoleId> ids, CancellationToken cancellationToken = default);
  Task<IEnumerable<RoleAggregate>> LoadAsync(IEnumerable<RoleId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<RoleAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);
  Task<IEnumerable<RoleAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<RoleAggregate?> LoadAsync(TenantId? tenantId, UniqueNameUnit uniqueName, CancellationToken cancellationToken = default);

  Task<IEnumerable<RoleAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken = default);

  Task SaveAsync(RoleAggregate role, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<RoleAggregate> roles, CancellationToken cancellationToken = default);
}
