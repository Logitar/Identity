namespace Logitar.Identity.Realms;

/// <summary>
/// Represents the settings used to validate a password in a realm.
/// </summary>
internal record PasswordSettings
{
  /// <summary>
  /// Gets or sets the minimum number of characters in a password.
  /// </summary>
  public int RequiredLength { get; init; }
  /// <summary>
  /// Gets or sets the required number of unique characters in a password.
  /// </summary>
  public int RequiredUniqueChars { get; init; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include at least one non-alphanumeric character (e.g. !"/$%?&*_+±@£¢¤¬¦²³¼½¾).
  /// </summary>
  public bool RequireNonAlphanumeric { get; init; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include a lowercase letter (a-z).
  /// </summary>
  public bool RequireLowercase { get; init; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include an uppercase letter (A-Z).
  /// </summary>
  public bool RequireUppercase { get; init; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include a digit (0-9).
  /// </summary>
  public bool RequireDigit { get; init; }
}
