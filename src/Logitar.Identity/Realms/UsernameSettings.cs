namespace Logitar.Identity.Realms;

/// <summary>
/// The output representation of the settings used to validate usernames in a realm.
/// </summary>
public record UsernameSettings
{
  /// <summary>
  /// Gets or sets the list of allowed characters in an username.
  /// </summary>
  public string? AllowedCharacters { get; set; }
}
