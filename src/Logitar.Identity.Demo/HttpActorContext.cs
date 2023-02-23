using Logitar.EventSourcing;
using Logitar.Identity.Actors;

namespace Logitar.Identity.Demo;

internal class HttpActorContext : IActorContext
{
  private static readonly Actor _system = new();

  public AggregateId ActorId => new(_system.Id);
}
