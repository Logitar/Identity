using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Roles.Events;

/// <summary>
/// Represents the event raised when a new <see cref="RoleAggregate"/> is created.
/// </summary>
public record RoleCreatedEvent : RoleSavedEvent, INotification
{
  /// <summary>
  /// Gets or sets the identifier of the realm in which the role belongs.
  /// </summary>
  public AggregateId RealmId { get; init; }

  /// <summary>
  /// Gets or sets the unique name of the role (case-insensitive).
  /// </summary>
  public string UniqueName { get; init; } = string.Empty;
}
