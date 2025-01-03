﻿using Logitar.Identity.Contracts.Settings;

namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate unique names.
/// </summary>
public record UniqueNameSettings : IUniqueNameSettings
{
  /// <summary>
  /// Gets or sets the list of allowed characters.
  /// </summary>
  public string? AllowedCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
}
