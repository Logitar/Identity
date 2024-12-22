using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class RoleRepository : Repository, IRoleRepository
{
  public RoleRepository(IEventStore eventStore) : base(eventStore)
  {
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

  public Task<IReadOnlyCollection<Role>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  public Task<Role?> LoadAsync(TenantId? tenantId, UniqueName uniqueName, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  public Task<IReadOnlyCollection<Role>> LoadAsync(ApiKey apiKey, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }
  public Task<IReadOnlyCollection<Role>> LoadAsync(User user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
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
