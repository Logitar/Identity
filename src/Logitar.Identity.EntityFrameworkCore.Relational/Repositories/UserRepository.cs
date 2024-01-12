using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class UserRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IUserRepository
{
  protected string AggregateType { get; } = typeof(UserAggregate).GetNamespaceQualifiedName();
  protected ISqlHelper SqlHelper { get; }

  public UserRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    SqlHelper = sqlHelper;
  }

  public async Task<UserAggregate?> LoadAsync(UserId id, CancellationToken cancellationToken)
    => await LoadAsync(id, version: null, cancellationToken);
  public async Task<UserAggregate?> LoadAsync(UserId id, long? version, CancellationToken cancellationToken)
    => await LoadAsync(id, version, includeDeleted: false, cancellationToken);
  public async Task<UserAggregate?> LoadAsync(UserId id, bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync(id, version: null, includeDeleted, cancellationToken);
  public async Task<UserAggregate?> LoadAsync(UserId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
    => await LoadAsync<UserAggregate>(id.AggregateId, version, includeDeleted, cancellationToken);

  public async Task<IEnumerable<UserAggregate>> LoadAsync(CancellationToken cancellationToken)
     => await LoadAsync(includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<UserAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken)
     => await LoadAsync<UserAggregate>(includeDeleted, cancellationToken);

  public async Task<IEnumerable<UserAggregate>> LoadAsync(IEnumerable<UserId> ids, CancellationToken cancellationToken = default)
    => await LoadAsync(ids, includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<UserAggregate>> LoadAsync(IEnumerable<UserId> ids, bool includeDeleted, CancellationToken cancellationToken = default)
  {
    IEnumerable<AggregateId> aggregateIds = ids.Select(id => id.AggregateId);
    return await LoadAsync<UserAggregate>(aggregateIds, includeDeleted, cancellationToken);
  }

  public async Task<IEnumerable<UserAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default)
    => await LoadAsync(tenantId, includeDeleted: false, cancellationToken);
  public async Task<IEnumerable<UserAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Users.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(IdentityDb.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Deserialize), includeDeleted);
  }

  public virtual async Task<UserAggregate?> LoadAsync(TenantId? tenantId, UniqueNameUnit uniqueName, CancellationToken cancellationToken)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Users.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(IdentityDb.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .Where(IdentityDb.Users.UniqueNameNormalized, Operators.IsEqualTo(uniqueName.Value.ToUpper()))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }
  public virtual async Task<IEnumerable<UserAggregate>> LoadAsync(TenantId? tenantId, EmailUnit email, CancellationToken cancellationToken)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Users.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(IdentityDb.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .Where(IdentityDb.Users.EmailAddressNormalized, Operators.IsEqualTo(email.Address.ToUpper()))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Deserialize));
  }
  public async Task<UserAggregate?> LoadAsync(TenantId? tenantId, string identifierKey, string identifierValue, CancellationToken cancellationToken)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Users.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Join(IdentityDb.UserIdentifiers.UserId, IdentityDb.Users.UserId)
      .Where(IdentityDb.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId.Value))
      .Where(IdentityDb.UserIdentifiers.Key, Operators.IsEqualTo(identifierKey.Trim()))
      .Where(IdentityDb.UserIdentifiers.Value, Operators.IsEqualTo(identifierValue.Trim()))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }

  public virtual async Task<IEnumerable<UserAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken)
  {
    IQuery query = SqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(IdentityDb.Users.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Join(IdentityDb.UserRoles.UserId, IdentityDb.Users.UserId)
      .Join(IdentityDb.Roles.RoleId, IdentityDb.UserRoles.RoleId)
      .Where(IdentityDb.Roles.AggregateId, Operators.IsEqualTo(role.Id.Value))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Deserialize));
  }
  public virtual async Task<UserAggregate> LoadAsync(SessionAggregate session, CancellationToken cancellationToken)
  {
    return await LoadAsync(session.UserId, cancellationToken)
      ?? throw new InvalidOperationException($"The user 'Id={session.UserId.Value}' could not be found.");
  }

  public virtual async Task SaveAsync(UserAggregate user, CancellationToken cancellationToken)
    => await base.SaveAsync(user, cancellationToken);
  public virtual async Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken)
    => await base.SaveAsync(users, cancellationToken);
}
