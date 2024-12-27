using Logitar.EventSourcing;
using Logitar.EventSourcing.Infrastructure;
using MediatR;

namespace Logitar.Identity.Infrastructure;

public class EventBus : IEventBus
{
  protected IMediator Mediator { get; }

  public EventBus(IMediator mediator)
  {
    Mediator = mediator;
  }

  public virtual async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
  {
    await Mediator.Publish(@event, cancellationToken);
  }
}
