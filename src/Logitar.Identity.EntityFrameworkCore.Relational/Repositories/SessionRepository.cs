using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class SessionRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, ISessionRepository
{
  public SessionRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    SqlHelper = sqlHelper;
  }

  protected string AggregateType { get; } = typeof(SessionAggregate).GetNamespaceQualifiedName();
  protected ISqlHelper SqlHelper { get; }

  public async Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken)
    => await LoadAsync(user, includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, bool includeDeleted, CancellationToken cancellationToken)
  {
    IQuery builder = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Sessions.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Join(IdentityDb.Users.UserId, IdentityDb.Sessions.UserId)
      .Where(IdentityDb.Users.AggregateId, Operators.IsEqualTo(user.Id.Value))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(builder)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<SessionAggregate>(events.Select(EventSerializer.Deserialize), includeDeleted);
  }

  public async Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken)
    => await base.SaveAsync(session, cancellationToken);
  public async Task SaveAsync(IEnumerable<SessionAggregate> sessions, CancellationToken cancellationToken)
    => await base.SaveAsync(sessions, cancellationToken);
}
