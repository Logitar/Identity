﻿using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Sessions.Events;

/// <summary>
/// The event raised when an existing session is modified.
/// </summary>
public record SessionUpdatedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the custom attribute modifications of the session.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; } = new();

  /// <summary>
  /// Gets a value indicating whether or not the session is being modified.
  /// </summary>
  public bool HasChanges => CustomAttributes.Any();
}