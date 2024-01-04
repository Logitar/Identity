using Logitar.EventSourcing;
using Logitar.Identity.Contracts.Actors;

namespace Logitar.Identity.Application.Caching;

internal interface ICacheService
{
  Actor? GetActor(ActorId id);
  void SetActor(Actor actor);
}
