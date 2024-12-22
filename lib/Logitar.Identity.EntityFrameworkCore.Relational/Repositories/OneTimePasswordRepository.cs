using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Passwords;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class OneTimePasswordRepository : Repository, IOneTimePasswordRepository
{
  public OneTimePasswordRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<OneTimePassword?> LoadAsync(OneTimePasswordId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, isDeleted: null, cancellationToken);
  }
  public async Task<OneTimePassword?> LoadAsync(OneTimePasswordId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version, isDeleted: null, cancellationToken);
  }
  public async Task<OneTimePassword?> LoadAsync(OneTimePasswordId id, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, isDeleted, cancellationToken);
  }
  public async Task<OneTimePassword?> LoadAsync(OneTimePasswordId id, long? version, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<OneTimePassword>(id.StreamId, version, isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<OneTimePassword>> LoadAsync(CancellationToken cancellationToken)
  {
    return await LoadAsync(isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<OneTimePassword>> LoadAsync(bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<OneTimePassword>(isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<OneTimePassword>> LoadAsync(IEnumerable<OneTimePasswordId> ids, CancellationToken cancellationToken)
  {
    return await LoadAsync(ids, isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<OneTimePassword>> LoadAsync(IEnumerable<OneTimePasswordId> ids, bool? isDeleted, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await LoadAsync<OneTimePassword>(streamIds, isDeleted, cancellationToken);
  }

  public Task<IReadOnlyCollection<OneTimePassword>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  public async Task SaveAsync(OneTimePassword onetimepassword, CancellationToken cancellationToken)
  {
    await base.SaveAsync(onetimepassword, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<OneTimePassword> onetimepasswords, CancellationToken cancellationToken)
  {
    await base.SaveAsync(onetimepasswords, cancellationToken);
  }
}
