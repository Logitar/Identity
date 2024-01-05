using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class UserRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IUserRepository
{
  public UserRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    SqlHelper = sqlHelper;
  }

  protected string AggregateType { get; } = typeof(UserAggregate).GetNamespaceQualifiedName();
  protected ISqlHelper SqlHelper { get; }

  public virtual async Task<UserAggregate?> LoadAsync(TenantId? tenantId, UniqueNameUnit uniqueName, CancellationToken cancellationToken)
  {
    IQuery builder = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Users.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(IdentityDb.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .Where(IdentityDb.Users.UniqueNameNormalized, Operators.IsEqualTo(uniqueName.Value.ToUpper()))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(builder)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }

  public virtual async Task SaveAsync(UserAggregate user, CancellationToken cancellationToken)
    => await base.SaveAsync(user, cancellationToken);
  public virtual async Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken)
    => await base.SaveAsync(users, cancellationToken);
}
