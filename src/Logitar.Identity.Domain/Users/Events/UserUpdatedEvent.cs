using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an existing user is modified.
/// </summary>
public record UserUpdatedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the display name of the user.
  /// </summary>
  public Modification<DisplayNameUnit>? DisplayName { get; internal set; }
  /// <summary>
  /// Gets or sets the description of the user.
  /// </summary>
  public Modification<DescriptionUnit>? Description { get; internal set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the user.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; } = new();

  /// <summary>
  /// Gets a value indicating whether or not the user is being modified.
  /// </summary>
  public bool HasChanges => DisplayName != null || Description != null || CustomAttributes.Any();
}
