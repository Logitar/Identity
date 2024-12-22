using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class SessionRepository : Repository, ISessionRepository
{
  public SessionRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Session?> LoadAsync(SessionId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, isDeleted: null, cancellationToken);
  }
  public async Task<Session?> LoadAsync(SessionId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version, isDeleted: null, cancellationToken);
  }
  public async Task<Session?> LoadAsync(SessionId id, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, isDeleted, cancellationToken);
  }
  public async Task<Session?> LoadAsync(SessionId id, long? version, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<Session>(id.StreamId, version, isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Session>> LoadAsync(CancellationToken cancellationToken)
  {
    return await LoadAsync(isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<Session>> LoadAsync(bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<Session>(isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Session>> LoadAsync(IEnumerable<SessionId> ids, CancellationToken cancellationToken)
  {
    return await LoadAsync(ids, isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<Session>> LoadAsync(IEnumerable<SessionId> ids, bool? isDeleted, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await LoadAsync<Session>(streamIds, isDeleted, cancellationToken);
  }

  public Task<IReadOnlyCollection<Session>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  public Task<IReadOnlyCollection<Session>> LoadActiveAsync(User user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }
  public Task<IReadOnlyCollection<Session>> LoadAsync(User user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  public async Task SaveAsync(Session session, CancellationToken cancellationToken)
  {
    await base.SaveAsync(session, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Session> sessions, CancellationToken cancellationToken)
  {
    await base.SaveAsync(sessions, cancellationToken);
  }
}
