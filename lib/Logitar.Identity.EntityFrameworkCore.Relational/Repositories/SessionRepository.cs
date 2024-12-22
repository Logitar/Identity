using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class SessionRepository : Repository, ISessionRepository
{
  private readonly IdentityContext _context;

  public SessionRepository(IdentityContext context, IEventStore eventStore) : base(eventStore)
  {
    _context = context;
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

  public async Task<IReadOnlyCollection<Session>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    string? tenantIdValue = tenantId?.Value;

    IEnumerable<StreamId> streamIds = (await _context.Sessions.AsNoTracking()
      .Where(x => x.TenantId == tenantIdValue)
      .Select(x => x.StreamId)
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<Session>(streamIds, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Session>> LoadActiveAsync(User user, CancellationToken cancellationToken)
  {
    string streamId = user.Id.Value;

    IEnumerable<StreamId> streamIds = (await _context.Sessions.AsNoTracking()
      .Include(x => x.User)
      .Where(x => x.IsActive && x.User!.StreamId == streamId)
      .Select(x => x.StreamId)
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<Session>(streamIds, cancellationToken);
  }
  public async Task<IReadOnlyCollection<Session>> LoadAsync(User user, CancellationToken cancellationToken)
  {
    string streamId = user.Id.Value;

    IEnumerable<StreamId> streamIds = (await _context.Sessions.AsNoTracking()
      .Include(x => x.User)
      .Where(x => x.User!.StreamId == streamId)
      .Select(x => x.StreamId)
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<Session>(streamIds, cancellationToken);
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
