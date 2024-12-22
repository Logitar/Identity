using Logitar.EventSourcing;
using Logitar.EventSourcing.Infrastructure;
using MediatR;

namespace Logitar.Identity.Infrastructure;

internal class EventBus : IEventBus
{
  private readonly IMediator _mediator;

  public EventBus(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
  {
    await _mediator.Publish(@event, cancellationToken);
  }
}
