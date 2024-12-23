using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Passwords;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class OneTimePasswordRepository : Repository, IOneTimePasswordRepository
{
  private readonly IdentityContext _context;

  public OneTimePasswordRepository(IdentityContext context, IEventStore eventStore) : base(eventStore)
  {
    _context = context;
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

  public async Task<IReadOnlyCollection<OneTimePassword>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    string? tenantIdValue = tenantId?.Value;

    IEnumerable<StreamId> streamIds = (await _context.OneTimePasswords.AsNoTracking()
      .Where(x => x.TenantId == tenantIdValue)
      .Select(x => x.StreamId)
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<OneTimePassword>(streamIds, cancellationToken);
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
