using Logitar.EventSourcing;
using Logitar.Identity.Actors;

namespace Logitar.Identity.Demo;

internal class HttpCurrentActor : ICurrentActor
{
  private static readonly Actor _system = new();

  public AggregateId Id => new(_system.Id);
}
