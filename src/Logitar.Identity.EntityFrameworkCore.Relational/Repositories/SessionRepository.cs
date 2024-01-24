using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class SessionRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, ISessionRepository
{
  protected string AggregateType { get; } = typeof(SessionAggregate).GetNamespaceQualifiedName();
  protected ISqlHelper SqlHelper { get; }

  public SessionRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    SqlHelper = sqlHelper;
  }

  public async Task<SessionAggregate?> LoadAsync(SessionId id, CancellationToken cancellationToken)
    => await LoadAsync(id, version: null, cancellationToken);
  public async Task<SessionAggregate?> LoadAsync(SessionId id, long? version, CancellationToken cancellationToken)
    => await LoadAsync(id, version, includeDeleted: false, cancellationToken);
  public async Task<SessionAggregate?> LoadAsync(SessionId id, bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync(id, version: null, includeDeleted, cancellationToken);
  public async Task<SessionAggregate?> LoadAsync(SessionId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync<SessionAggregate>(id.AggregateId, version, includeDeleted, cancellationToken);

  public async Task<IEnumerable<SessionAggregate>> LoadAsync(CancellationToken cancellationToken)
    => await LoadAsync(includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<SessionAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync<SessionAggregate>(includeDeleted, cancellationToken);

  public async Task<IEnumerable<SessionAggregate>> LoadAsync(IEnumerable<SessionId> ids, CancellationToken cancellationToken)
    => await LoadAsync(ids, includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<SessionAggregate>> LoadAsync(IEnumerable<SessionId> ids, bool includeDeleted, CancellationToken cancellationToken)
  {
    IEnumerable<AggregateId> aggregateIds = ids.Select(id => id.AggregateId);
    return await LoadAsync<SessionAggregate>(aggregateIds, includeDeleted, cancellationToken);
  }

  public async Task<IEnumerable<SessionAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Sessions.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Join(IdentityDb.Users.UserId, IdentityDb.Sessions.UserId)
      .Where(IdentityDb.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<SessionAggregate>(events.Select(EventSerializer.Deserialize));
  }

  public async Task<IEnumerable<SessionAggregate>> LoadActiveAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Sessions.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Join(IdentityDb.Users.UserId, IdentityDb.Sessions.UserId)
      .Where(IdentityDb.Users.AggregateId, Operators.IsEqualTo(user.Id.Value))
      .Where(IdentityDb.Sessions.IsActive, Operators.IsEqualTo(true))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<SessionAggregate>(events.Select(EventSerializer.Deserialize));
  }
  public async Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Sessions.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Join(IdentityDb.Users.UserId, IdentityDb.Sessions.UserId)
      .Where(IdentityDb.Users.AggregateId, Operators.IsEqualTo(user.Id.Value))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<SessionAggregate>(events.Select(EventSerializer.Deserialize));
  }

  public async Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken)
    => await base.SaveAsync(session, cancellationToken);
  public async Task SaveAsync(IEnumerable<SessionAggregate> sessions, CancellationToken cancellationToken)
    => await base.SaveAsync(sessions, cancellationToken);
}
