using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class RoleRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IRoleRepository
{
  protected string AggregateType { get; } = typeof(RoleAggregate).GetNamespaceQualifiedName();
  protected ISqlHelper SqlHelper { get; }

  public RoleRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    SqlHelper = sqlHelper;
  }

  public async Task<RoleAggregate?> LoadAsync(RoleId id, CancellationToken cancellationToken)
    => await LoadAsync(id, version: null, cancellationToken);
  public async Task<RoleAggregate?> LoadAsync(RoleId id, long? version, CancellationToken cancellationToken)
    => await LoadAsync(id, version, includeDeleted: false, cancellationToken);
  public async Task<RoleAggregate?> LoadAsync(RoleId id, bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync(id, version: null, includeDeleted, cancellationToken);
  public async Task<RoleAggregate?> LoadAsync(RoleId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync<RoleAggregate>(id.AggregateId, version, includeDeleted, cancellationToken);

  public async Task<IEnumerable<RoleAggregate>> LoadAsync(CancellationToken cancellationToken)
     => await LoadAsync(includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<RoleAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken)
     => await LoadAsync<RoleAggregate>(includeDeleted, cancellationToken);

  public async Task<IEnumerable<RoleAggregate>> LoadAsync(IEnumerable<RoleId> ids, CancellationToken cancellationToken = default)
    => await LoadAsync(ids, includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<RoleAggregate>> LoadAsync(IEnumerable<RoleId> ids, bool includeDeleted, CancellationToken cancellationToken = default)
  {
    IEnumerable<AggregateId> aggregateIds = ids.Select(id => id.AggregateId);
    return await LoadAsync<RoleAggregate>(aggregateIds, includeDeleted, cancellationToken);
  }

  public async Task<IEnumerable<RoleAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Roles.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(IdentityDb.Roles.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<RoleAggregate>(events.Select(EventSerializer.Deserialize));
  }

  public virtual async Task<RoleAggregate?> LoadAsync(TenantId? tenantId, UniqueNameUnit uniqueName, CancellationToken cancellationToken)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Roles.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(IdentityDb.Roles.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .Where(IdentityDb.Roles.UniqueNameNormalized, Operators.IsEqualTo(uniqueName.Value.ToUpper()))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<RoleAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }

  public virtual async Task<IEnumerable<RoleAggregate>> LoadAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken)
    => await LoadAsync(apiKey.Roles, cancellationToken);
  public virtual async Task<IEnumerable<RoleAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken)
    => await LoadAsync(user.Roles, cancellationToken);

  public virtual async Task SaveAsync(RoleAggregate role, CancellationToken cancellationToken)
    => await base.SaveAsync(role, cancellationToken);
  public virtual async Task SaveAsync(IEnumerable<RoleAggregate> roles, CancellationToken cancellationToken)
    => await base.SaveAsync(roles, cancellationToken);
}
