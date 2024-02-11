using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Roles.Events;

/// <summary>
/// The event raised when an existing role is modified.
/// </summary>
public record RoleUpdatedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the display name of the role.
  /// </summary>
  public Modification<DisplayNameUnit>? DisplayName { get; set; }
  /// <summary>
  /// Gets or sets the description of the role.
  /// </summary>
  public Modification<DescriptionUnit>? Description { get; set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the role.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; init; } = [];

  /// <summary>
  /// Gets a value indicating whether or not the role is being modified.
  /// </summary>
  [JsonIgnore]
  public bool HasChanges => DisplayName != null || Description != null || CustomAttributes.Count > 0;
}
