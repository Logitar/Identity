using MediatR;

namespace Logitar.Identity.Realms.Events;

/// <summary>
/// The event raised when a new <see cref="RealmAggregate"/> is created.
/// </summary>
public record RealmCreatedEvent : RealmSavedEvent, INotification
{
  /// <summary>
  /// Gets or sets the unique name of the realm (case-insensitive).
  /// </summary>
  public string UniqueName { get; init; } = string.Empty;
}
