using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Users.Events;

/// <summary>
/// Represents the event raised when a new <see cref="UserAggregate"/> is created.
/// </summary>
public record UserCreatedEvent : UserSavedEvent, INotification
{
  /// <summary>
  /// Gets or sets the identifier of the realm in which the user belongs.
  /// </summary>
  public AggregateId RealmId { get; init; }

  /// <summary>
  /// Gets or sets the unique name of the user.
  /// </summary>
  public string Username { get; init; } = string.Empty;
}
