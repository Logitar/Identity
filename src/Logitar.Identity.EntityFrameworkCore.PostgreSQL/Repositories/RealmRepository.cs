using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL;
using Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.Realms;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Repositories;

/// <summary>
/// Implements methods to load realms from the event store.
/// </summary>
internal class RealmRepository : EventStore, IRealmRepository
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RealmRepository"/> class using the specified arguments.
  /// </summary>
  /// <param name="context">The event database context.</param>
  /// <param name="eventBus">The event bus.</param>
  public RealmRepository(EventContext context, IEventBus eventBus) : base(context, eventBus)
  {
  }

  /// <summary>
  /// Retrieves a realm by its identifier or unique name.
  /// </summary>
  /// <param name="idOrUniqueName">The identifier or unique name of the realm.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The realm or null if not found.</returns>
  public async Task<RealmAggregate?> LoadAsync(string idOrUniqueName, CancellationToken cancellationToken)
  {
    string aggregateId = (Guid.TryParse(idOrUniqueName, out Guid realmId)
      ? new AggregateId(realmId)
      : new AggregateId(idOrUniqueName)).Value;
    string aggregateType = typeof(RealmAggregate).GetName();

    EventEntity[] events = await Context.Events.FromSqlInterpolated($@"SELECT e.* FROM ""Events"" e JOIN ""Realms"" r on r.""AggregateId"" = e.""AggregateId"" WHERE e.""AggregateType"" = {aggregateType} AND (r.""AggregateId"" = {aggregateId} OR r.""UniqueNameNormalized"" = {idOrUniqueName.ToUpper()})")
      .AsNoTracking()
      .OrderBy(x => x.Version)
      .ToArrayAsync(cancellationToken);

    return Load<RealmAggregate>(events).SingleOrDefault();
  }

  /// <summary>
  /// Retrieves a realm by its unique name.
  /// </summary>
  /// <param name="uniqueName">The unique name of the realm.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The realm or null if not found.</returns>
  public async Task<RealmAggregate?> LoadByUniqueNameAsync(string uniqueName, CancellationToken cancellationToken)
  {
    string aggregateType = typeof(RealmAggregate).GetName();

    EventEntity[] events = await Context.Events.FromSqlInterpolated($@"SELECT e.* FROM ""Events"" e JOIN ""Realms"" r on r.""AggregateId"" = e.""AggregateId"" WHERE e.""AggregateType"" = {aggregateType} AND r.""UniqueNameNormalized"" = {uniqueName.ToUpper()}")
      .AsNoTracking()
      .OrderBy(x => x.Version)
      .ToArrayAsync(cancellationToken);

    return Load<RealmAggregate>(events).SingleOrDefault();
  }
}
