namespace Logitar.Identity.Realms;

/// <summary>
/// The output representation of the settings used to validate usernames in a realm.
/// </summary>
public record PasswordSettings
{
  /// <summary>
  /// Gets or sets the minimum number of characters in a password.
  /// </summary>
  public int RequiredLength { get; set; }
  /// <summary>
  /// Gets or sets the required number of unique characters in a password.
  /// </summary>
  public int RequiredUniqueChars { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include at least one non-alphanumeric character (e.g. !"/$%?&*_+±@£¢¤¬¦²³¼½¾).
  /// </summary>
  public bool RequireNonAlphanumeric { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include a lowercase letter (a-z).
  /// </summary>
  public bool RequireLowercase { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include an uppercase letter (A-Z).
  /// </summary>
  public bool RequireUppercase { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include a digit (0-9).
  /// </summary>
  public bool RequireDigit { get; set; }
}
