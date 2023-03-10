using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL;
using Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.ApiKeys;
using Logitar.Identity.Realms;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Repositories;

/// <summary>
/// Implements methods to load API keys from the event store.
/// </summary>
internal class ApiKeyRepository : EventStore, IApiKeyRepository
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserRepository"/> class using the specified arguments.
  /// </summary>
  /// <param name="context">The event database context.</param>
  /// <param name="eventBus">The event bus.</param>
  public ApiKeyRepository(EventContext context, IEventBus eventBus) : base(context, eventBus)
  {
  }

  /// <summary>
  /// Retrieves the list of users in the specified realm.
  /// </summary>
  /// <param name="realm">The realm the users belong to.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of users, or empty if none.</returns>
  public async Task<IEnumerable<ApiKeyAggregate>> LoadAsync(RealmAggregate realm, CancellationToken cancellationToken)
  {
    string aggregateType = typeof(ApiKeyAggregate).GetName();

    EventEntity[] events = await Context.Events.FromSqlInterpolated($@"SELECT e.* FROM ""Events"" e JOIN ""ApiKeys"" a on a.""AggregateId"" = e.""AggregateId"" JOIN ""Realms"" r ON r.""RealmId"" = a.""RealmId"" WHERE e.""AggregateType"" = {aggregateType} AND r.""AggregateId"" = {realm.Id.Value}")
      .AsNoTracking()
      .OrderBy(x => x.Version)
      .ToArrayAsync(cancellationToken);

    return Load<ApiKeyAggregate>(events);
  }
}
