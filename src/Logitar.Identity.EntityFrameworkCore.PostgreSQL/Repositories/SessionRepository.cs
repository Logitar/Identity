using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL;
using Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.Sessions;
using Logitar.Identity.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Repositories;

/// <summary>
/// Implements methods to load user sessions from the event store.
/// </summary>
internal class SessionRepository : EventStore, ISessionRepository
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserRepository"/> class using the specified arguments.
  /// </summary>
  /// <param name="context">The event database context.</param>
  /// <param name="eventBus">The event bus.</param>
  public SessionRepository(EventContext context, IEventBus eventBus) : base(context, eventBus)
  {
  }

  /// <summary>
  /// Retrieves the list of the active sessions of the specified user.
  /// </summary>
  /// <param name="user">The user to retrieve its sessions.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of active sessions, or empty if none.</returns>
  public async Task<IEnumerable<SessionAggregate>> LoadActiveAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    string aggregateType = typeof(SessionAggregate).GetName();

    EventEntity[] events = await Context.Events.FromSqlInterpolated($@"SELECT e.* FROM ""Events"" e JOIN ""Sessions"" s ON s.""AggregateId"" = e.""AggregateId"" JOIN ""Users"" u on u.""UserId"" = s.""UserId"" WHERE e.""AggregateType"" = {aggregateType} AND s.""IsActive"" = true AND u.""AggregateId"" = {user.Id.Value}")
      .AsNoTracking()
      .OrderBy(x => x.Version)
      .ToArrayAsync(cancellationToken);

    return Load<SessionAggregate>(events);
  }
}
