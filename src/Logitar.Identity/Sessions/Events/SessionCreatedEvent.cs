using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Sessions.Events;

/// <summary>
/// Represents the event raised when a new <see cref="SessionAggregate"/> is created.
/// </summary>
public record SessionCreatedEvent : SessionSavedEvent, INotification
{
  /// <summary>
  /// Gets or sets the identifier of the user to whom the session belongs.
  /// </summary>
  public AggregateId UserId { get; init; }
}
