using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class ApiKeyRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IApiKeyRepository
{
  protected string AggregateType { get; } = typeof(ApiKeyAggregate).GetNamespaceQualifiedName();
  protected ISqlHelper SqlHelper { get; }

  public ApiKeyRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    SqlHelper = sqlHelper;
  }

  public async Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, CancellationToken cancellationToken)
    => await LoadAsync(id, version: null, cancellationToken);
  public async Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, long? version, CancellationToken cancellationToken)
    => await LoadAsync(id, version, includeDeleted: false, cancellationToken);
  public async Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync(id, version: null, includeDeleted, cancellationToken);
  public async Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync<ApiKeyAggregate>(id.AggregateId, version, includeDeleted, cancellationToken);

  public async Task<IEnumerable<ApiKeyAggregate>> LoadAsync(CancellationToken cancellationToken)
     => await LoadAsync(includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<ApiKeyAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken)
     => await LoadAsync<ApiKeyAggregate>(includeDeleted, cancellationToken);

  public async Task<IEnumerable<ApiKeyAggregate>> LoadAsync(IEnumerable<ApiKeyId> ids, CancellationToken cancellationToken = default)
    => await LoadAsync(ids, includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<ApiKeyAggregate>> LoadAsync(IEnumerable<ApiKeyId> ids, bool includeDeleted, CancellationToken cancellationToken = default)
  {
    IEnumerable<AggregateId> aggregateIds = ids.Select(id => id.AggregateId);
    return await LoadAsync<ApiKeyAggregate>(aggregateIds, includeDeleted, cancellationToken);
  }

  public async Task<IEnumerable<ApiKeyAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default)
    => await LoadAsync(tenantId, includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<ApiKeyAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.ApiKeys.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(IdentityDb.ApiKeys.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<ApiKeyAggregate>(events.Select(EventSerializer.Deserialize));
  }

  public virtual async Task<IEnumerable<ApiKeyAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.ApiKeys.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateId, Operators.IsEqualTo(AggregateType))
      )
      .Join(IdentityDb.ApiKeyRoles.ApiKeyId, IdentityDb.ApiKeys.ApiKeyId)
      .Join(IdentityDb.Roles.RoleId, IdentityDb.ApiKeyRoles.RoleId)
      .Where(IdentityDb.Roles.AggregateId, Operators.IsEqualTo(role.Id.Value))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<ApiKeyAggregate>(events.Select(EventSerializer.Deserialize));
  }

  public virtual async Task SaveAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken)
    => await base.SaveAsync(apiKey, cancellationToken);
  public virtual async Task SaveAsync(IEnumerable<ApiKeyAggregate> apiKeys, CancellationToken cancellationToken)
    => await base.SaveAsync(apiKeys, cancellationToken);
}
