using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

/// <summary>
/// Represents an event bus, where events will be published.
/// </summary>
public class EventBus : IEventBus
{
  /// <summary>
  /// The publisher instance.
  /// </summary>
  private readonly IPublisher _publisher;

  /// <summary>
  /// Initializes a new instance of the <see cref="EventBus"/> class using the specified publisher.
  /// </summary>
  /// <param name="publisher">The publisher instance.</param>
  public EventBus(IPublisher publisher)
  {
    _publisher = publisher;
  }

  /// <summary>
  /// Publishes the specified event.
  /// </summary>
  /// <param name="change">The event to publish.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public virtual async Task PublishAsync(DomainEvent change, CancellationToken cancellationToken)
  {
    await _publisher.Publish(change, cancellationToken);
  }
}
