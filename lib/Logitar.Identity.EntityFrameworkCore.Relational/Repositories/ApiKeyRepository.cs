using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Roles;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class ApiKeyRepository : Repository, IApiKeyRepository
{
  private readonly IdentityContext _context;

  public ApiKeyRepository(IdentityContext context, IEventStore eventStore) : base(eventStore)
  {
    _context = context;
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

  public async Task<IReadOnlyCollection<ApiKey>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    string? tenantIdValue = tenantId?.Value;

    IEnumerable<StreamId> streamIds = (await _context.ApiKeys.AsNoTracking()
      .Where(x => x.TenantId == tenantIdValue)
      .Select(x => x.StreamId)
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<ApiKey>(streamIds, cancellationToken);
  }

  public async Task<IReadOnlyCollection<ApiKey>> LoadAsync(Role role, CancellationToken cancellationToken)
  {
    string streamId = role.Id.Value;

    IEnumerable<StreamId> streamIds = (await _context.ApiKeys.AsNoTracking()
      .Include(x => x.Roles)
      .Where(x => x.Roles.Any(role => role.StreamId == streamId))
      .Select(x => x.StreamId)
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<ApiKey>(streamIds, cancellationToken);
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
