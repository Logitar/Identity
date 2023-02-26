using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Sessions.Events;

/// <summary>
/// Represents the event raised when a new <see cref="SessionAggregate"/> is created.
/// </summary>
public record SessionCreatedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the identifier of the user to whom the session belongs.
  /// </summary>
  public AggregateId UserId { get; init; }

  /// <summary>
  /// Gets or sets the salted and hashed key of the user session.
  /// </summary>
  public string? KeyHash { get; init; }

  /// <summary>
  /// Gets or sets the custom attributes of the user session.
  /// </summary>
  public Dictionary<string, string> CustomAttributes { get; init; } = new();
}
