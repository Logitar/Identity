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
    string uniqueNameNormalized = uniqueName.Value.ToUpper();

    IQuery query = SqlHelper.QueryFrom(Db.Events.Table)
      .Join(Db.Users.AggregateId, Db.Events.AggregateId, new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType)))
      .Where(Db.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .Where(Db.Users.UniqueNameNormalized, Operators.IsEqualTo(uniqueNameNormalized))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<UserAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }
  public virtual async Task<IEnumerable<UserAggregate>> LoadAsync(TenantId? tenantId, EmailUnit email, CancellationToken cancellationToken)
  {
    string emailAddressNormalized = email.Address.ToUpper();

    IQuery query = SqlHelper.QueryFrom(Db.Events.Table)
      .Join(Db.Users.AggregateId, Db.Events.AggregateId, new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType)))
      .Where(Db.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .Where(Db.Users.EmailAddressNormalized, Operators.IsEqualTo(emailAddressNormalized))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<UserAggregate>(events.Select(EventSerializer.Deserialize));
  }

  public virtual async Task SaveAsync(UserAggregate user, CancellationToken cancellationToken)
    => await base.SaveAsync(user, cancellationToken);
  public virtual async Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken)
    => await base.SaveAsync(users, cancellationToken);
}
