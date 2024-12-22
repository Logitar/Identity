using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class UserRepository : Repository, IUserRepository
{
  public UserRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<User?> LoadAsync(UserId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, isDeleted: null, cancellationToken);
  }
  public async Task<User?> LoadAsync(UserId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version, isDeleted: null, cancellationToken);
  }
  public async Task<User?> LoadAsync(UserId id, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, isDeleted, cancellationToken);
  }
  public async Task<User?> LoadAsync(UserId id, long? version, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<User>(id.StreamId, version, isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<User>> LoadAsync(CancellationToken cancellationToken)
  {
    return await LoadAsync(isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<User>> LoadAsync(bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<User>(isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<User>> LoadAsync(IEnumerable<UserId> ids, CancellationToken cancellationToken)
  {
    return await LoadAsync(ids, isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<User>> LoadAsync(IEnumerable<UserId> ids, bool? isDeleted, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await LoadAsync<User>(streamIds, isDeleted, cancellationToken);
  }

  public Task<IReadOnlyCollection<User>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  public Task<User?> LoadAsync(TenantId? tenantId, UniqueName uniqueName, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }
  public Task<IReadOnlyCollection<User>> LoadAsync(TenantId? tenantId, Email email, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }
  public Task<User?> LoadAsync(TenantId? tenantId, Identifier identifierKey, CustomIdentifier identifierValue, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  public Task<IReadOnlyCollection<User>> LoadAsync(Role role, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }
  public async Task<User> LoadAsync(Session session, CancellationToken cancellationToken)
  {
    return await LoadAsync<User>(session.UserId.StreamId, cancellationToken)
      ?? throw new InvalidOperationException($"The user aggregate 'Id={session.UserId}' could not be loaded.");
  }

  public async Task SaveAsync(User user, CancellationToken cancellationToken)
  {
    await base.SaveAsync(user, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<User> users, CancellationToken cancellationToken)
  {
    await base.SaveAsync(users, cancellationToken);
  }
}
