using Logitar.EventSourcing;
using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Logitar.Identity.Core.Roles.Events;

/// <summary>
/// The event raised when a role is updated.
/// </summary>
public record RoleUpdated : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the new display name of the role.
  /// </summary>
  public Change<DisplayName>? DisplayName { get; set; }
  /// <summary>
  /// Gets or sets the new description of the role.
  /// </summary>
  public Change<Description>? Description { get; set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the role.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; init; } = [];

  /// <summary>
  /// Gets a value indicating whether or not the role has been updated.
  /// </summary>
  [JsonIgnore]
  public bool HasChanges => DisplayName != null || Description != null || CustomAttributes.Count > 0;
}
