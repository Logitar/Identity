using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class RoleRepository : Repository, IRoleRepository
{
  private readonly IdentityContext _context;

  public RoleRepository(IdentityContext context, IEventStore eventStore) : base(eventStore)
  {
    _context = context;
  }

  public async Task<Role?> LoadAsync(RoleId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, isDeleted: null, cancellationToken);
  }
  public async Task<Role?> LoadAsync(RoleId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version, isDeleted: null, cancellationToken);
  }
  public async Task<Role?> LoadAsync(RoleId id, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, isDeleted, cancellationToken);
  }
  public async Task<Role?> LoadAsync(RoleId id, long? version, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<Role>(id.StreamId, version, isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Role>> LoadAsync(CancellationToken cancellationToken)
  {
    return await LoadAsync(isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<Role>> LoadAsync(bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<Role>(isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Role>> LoadAsync(IEnumerable<RoleId> ids, CancellationToken cancellationToken)
  {
    return await LoadAsync(ids, isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<Role>> LoadAsync(IEnumerable<RoleId> ids, bool? isDeleted, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await LoadAsync<Role>(streamIds, isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Role>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    string? tenantIdValue = tenantId?.Value;

    IEnumerable<StreamId> streamIds = (await _context.Roles.AsNoTracking()
      .Where(x => x.TenantId == tenantIdValue)
      .Select(x => x.StreamId)
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<Role>(streamIds, cancellationToken);
  }

  public async Task<Role?> LoadAsync(TenantId? tenantId, UniqueName uniqueName, CancellationToken cancellationToken)
  {
    string? tenantIdValue = tenantId?.Value;
    string uniqueNameNormalized = IdentityDb.Helper.Normalize(uniqueName.Value);

    string? streamId = await _context.Roles.AsNoTracking()
      .Where(x => x.TenantId == tenantIdValue && x.UniqueNameNormalized == uniqueNameNormalized)
      .Select(x => x.StreamId)
      .SingleOrDefaultAsync(cancellationToken);
    if (streamId == null)
    {
      return null;
    }

    return await LoadAsync<Role>(new StreamId(streamId), cancellationToken);
  }

  public async Task<IReadOnlyCollection<Role>> LoadAsync(ApiKey apiKey, CancellationToken cancellationToken)
  {
    string streamId = apiKey.Id.Value;

    IEnumerable<StreamId> streamIds = (await _context.ApiKeys.AsNoTracking()
      .Include(x => x.Roles)
      .Where(x => x.StreamId == streamId)
      .SelectMany(x => x.Roles.Select(role => role.StreamId))
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<Role>(streamIds, cancellationToken);
  }
  public async Task<IReadOnlyCollection<Role>> LoadAsync(User user, CancellationToken cancellationToken)
  {
    string streamId = user.Id.Value;

    IEnumerable<StreamId> streamIds = (await _context.Users.AsNoTracking()
      .Include(x => x.Roles)
      .Where(x => x.StreamId == streamId)
      .SelectMany(x => x.Roles.Select(role => role.StreamId))
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<Role>(streamIds, cancellationToken);
  }

  public async Task SaveAsync(Role role, CancellationToken cancellationToken)
  {
    await base.SaveAsync(role, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Role> roles, CancellationToken cancellationToken)
  {
    await base.SaveAsync(roles, cancellationToken);
  }
}
