﻿using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Roles.Events;

/// <summary>
/// The event raised when an existing role is modified.
/// </summary>
public record RoleUpdatedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the display name of the role.
  /// </summary>
  public Modification<DisplayNameUnit>? DisplayName { get; internal set; }
  /// <summary>
  /// Gets or sets the description of the role.
  /// </summary>
  public Modification<DescriptionUnit>? Description { get; internal set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the role.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; } = new();

  /// <summary>
  /// Gets a value indicating whether or not the role is being modified.
  /// </summary>
  public bool HasChanges => DisplayName != null || Description != null || CustomAttributes.Any();
}
