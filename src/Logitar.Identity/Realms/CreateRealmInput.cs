﻿namespace Logitar.Identity.Realms;

/// <summary>
/// The realm creation input data.
/// </summary>
public record CreateRealmInput : SaveRealmInput
{
  /// <summary>
  /// Gets or sets the unique name of the realm (case-insensitive).
  /// </summary>
  public string UniqueName { get; set; } = string.Empty;
}
