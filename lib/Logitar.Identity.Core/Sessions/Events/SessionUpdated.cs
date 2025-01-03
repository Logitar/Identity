﻿using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Events;

/// <summary>
/// The event raised when an existing session is modified.
/// </summary>
public record SessionUpdated : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the custom attribute modifications of the session.
  /// </summary>
  public Dictionary<Identifier, string?> CustomAttributes { get; init; } = [];

  /// <summary>
  /// Gets a value indicating whether or not the session is being modified.
  /// </summary>
  [JsonIgnore]
  public bool HasChanges => CustomAttributes.Count > 0;
}
