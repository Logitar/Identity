using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Roles;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class ApiKeyRepository : Repository, IApiKeyRepository
{
  public ApiKeyRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<ApiKey?> LoadAsync(ApiKeyId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, isDeleted: null, cancellationToken);
  }
  public async Task<ApiKey?> LoadAsync(ApiKeyId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version, isDeleted: null, cancellationToken);
  }
  public async Task<ApiKey?> LoadAsync(ApiKeyId id, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, isDeleted, cancellationToken);
  }
  public async Task<ApiKey?> LoadAsync(ApiKeyId id, long? version, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<ApiKey>(id.StreamId, version, isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<ApiKey>> LoadAsync(CancellationToken cancellationToken)
  {
    return await LoadAsync(isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<ApiKey>> LoadAsync(bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<ApiKey>(isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<ApiKey>> LoadAsync(IEnumerable<ApiKeyId> ids, CancellationToken cancellationToken)
  {
    return await LoadAsync(ids, isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<ApiKey>> LoadAsync(IEnumerable<ApiKeyId> ids, bool? isDeleted, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await LoadAsync<ApiKey>(streamIds, isDeleted, cancellationToken);
  }

  public Task<IReadOnlyCollection<ApiKey>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  public Task<IReadOnlyCollection<ApiKey>> LoadAsync(Role role, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  public async Task SaveAsync(ApiKey apikey, CancellationToken cancellationToken)
  {
    await base.SaveAsync(apikey, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<ApiKey> apikeys, CancellationToken cancellationToken)
  {
    await base.SaveAsync(apikeys, cancellationToken);
  }
}
