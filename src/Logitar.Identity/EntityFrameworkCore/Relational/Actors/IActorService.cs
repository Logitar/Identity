using Logitar.EventSourcing;
using Logitar.Identity.Contracts.Actors;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Actors;

internal interface IActorService
{
  Task<IEnumerable<Actor>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken = default);
}
