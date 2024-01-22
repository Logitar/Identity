using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class OneTimePasswordRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IOneTimePasswordRepository
{
  protected string AggregateType { get; } = typeof(OneTimePasswordAggregate).GetNamespaceQualifiedName();
  protected ISqlHelper SqlHelper { get; }

  public OneTimePasswordRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    SqlHelper = sqlHelper;
  }

  public async Task<OneTimePasswordAggregate?> LoadAsync(OneTimePasswordId id, CancellationToken cancellationToken)
    => await LoadAsync(id, version: null, cancellationToken);
  public async Task<OneTimePasswordAggregate?> LoadAsync(OneTimePasswordId id, long? version, CancellationToken cancellationToken)
    => await LoadAsync(id, version, includeDeleted: false, cancellationToken);
  public async Task<OneTimePasswordAggregate?> LoadAsync(OneTimePasswordId id, bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync(id, version: null, includeDeleted, cancellationToken);
  public async Task<OneTimePasswordAggregate?> LoadAsync(OneTimePasswordId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync<OneTimePasswordAggregate>(id.AggregateId, version, includeDeleted, cancellationToken);

  public async Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(CancellationToken cancellationToken)
     => await LoadAsync(includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken)
     => await LoadAsync<OneTimePasswordAggregate>(includeDeleted, cancellationToken);

  public async Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(IEnumerable<OneTimePasswordId> ids, CancellationToken cancellationToken = default)
    => await LoadAsync(ids, includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(IEnumerable<OneTimePasswordId> ids, bool includeDeleted, CancellationToken cancellationToken = default)
  {
    IEnumerable<AggregateId> aggregateIds = ids.Select(id => id.AggregateId);
    return await LoadAsync<OneTimePasswordAggregate>(aggregateIds, includeDeleted, cancellationToken);
  }

  public async Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default)
    => await LoadAsync(tenantId, includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.OneTimePasswords.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(IdentityDb.OneTimePasswords.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<OneTimePasswordAggregate>(events.Select(EventSerializer.Deserialize), includeDeleted);
  }

  public virtual async Task SaveAsync(OneTimePasswordAggregate oneTimePassword, CancellationToken cancellationToken)
    => await base.SaveAsync(oneTimePassword, cancellationToken);
  public virtual async Task SaveAsync(IEnumerable<OneTimePasswordAggregate> oneTimePasswords, CancellationToken cancellationToken)
    => await base.SaveAsync(oneTimePasswords, cancellationToken);
}
