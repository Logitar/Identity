namespace Logitar.Identity.Realms;

/// <summary>
/// Represents the settings used to validate an username in a realm.
/// </summary>
internal record UsernameSettings
{
  /// <summary>
  /// Gets or sets the list of allowed characters in an username.
  /// </summary>
  public string? AllowedCharacters { get; init; }
}
