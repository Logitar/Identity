﻿using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Roles.Events;

/// <summary>
/// The base <see cref="RoleAggregate"/> save event.
/// </summary>
public abstract record RoleSavedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the display name of the role.
  /// </summary>
  public string? DisplayName { get; init; }
  /// <summary>
  /// Gets or sets a textual description for the role.
  /// </summary>
  public string? Description { get; init; }

  /// <summary>
  /// Gets or sets the custom attributes of the role.
  /// </summary>
  public Dictionary<string, string> CustomAttributes { get; init; } = new();
}
