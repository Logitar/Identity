using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys;

public interface IApiKeyRepository
{
  Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, CancellationToken cancellationToken = default);
  Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, long? version, CancellationToken cancellationToken = default);
  Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, bool includeDeleted, CancellationToken cancellationToken = default);
  Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(IEnumerable<ApiKeyId> ids, CancellationToken cancellationToken = default);
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(IEnumerable<ApiKeyId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken = default);

  Task SaveAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<ApiKeyAggregate> apiKeys, CancellationToken cancellationToken = default);
}
