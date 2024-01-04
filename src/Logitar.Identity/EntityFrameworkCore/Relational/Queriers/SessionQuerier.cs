using Logitar.EventSourcing;
using Logitar.Identity.Application.Sessions;
using Logitar.Identity.Contracts.Actors;
using Logitar.Identity.Contracts.Sessions;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Queriers;

internal class SessionQuerier : ISessionQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<SessionEntity> _sessions;

  public SessionQuerier(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _sessions = context.Sessions;
  }

  public async Task<Session> ReadAsync(SessionAggregate session, CancellationToken cancellationToken)
  {
    return await ReadAsync(session.Id.AggregateId.ToGuid(), cancellationToken)
      ?? throw new InvalidOperationException($"The session 'AggregateId={session.Id.Value}' could not be found.");
  }
  public async Task<Session?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    string aggregateId = new AggregateId(id).Value;

    SessionEntity? session = await _sessions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == aggregateId, cancellationToken);

    return session == null ? null : await MapAsync(session, cancellationToken);
  }

  private async Task<Session> MapAsync(SessionEntity session, CancellationToken cancellationToken)
    => (await MapAsync(new[] { session }, cancellationToken)).Single();
  private async Task<IEnumerable<Session>> MapAsync(IEnumerable<SessionEntity> sessions, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = sessions.SelectMany(session => session.GetActorIds());
    IEnumerable<Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return sessions.Select(mapper.ToSession);
  }
}
