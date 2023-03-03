using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Sessions.Events;

/// <summary>
/// The base <see cref="SessionAggregate"/> save event.
/// </summary>
public abstract record SessionSavedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the salted and hashed key of the user session.
  /// </summary>
  public string? KeyHash { get; init; }

  /// <summary>
  /// Gets or sets the custom attributes of the user session.
  /// </summary>
  public Dictionary<string, string> CustomAttributes { get; init; } = new();
}
