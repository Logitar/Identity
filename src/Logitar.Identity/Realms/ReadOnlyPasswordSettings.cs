namespace Logitar.Identity.Realms;

/// <summary>
/// Represents the settings used to validate a password in a realm.
/// </summary>
public record ReadOnlyPasswordSettings
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyPasswordSettings"/> class.
  /// </summary>
  public ReadOnlyPasswordSettings()
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyPasswordSettings"/> class using the specified arguments.
  /// </summary>
  /// <param name="settings">The password settings.</param>
  public ReadOnlyPasswordSettings(PasswordSettings settings)
  {
    RequiredLength = settings.RequiredLength;
    RequiredUniqueChars = settings.RequiredUniqueChars;
    RequireNonAlphanumeric = settings.RequireNonAlphanumeric;
    RequireLowercase = settings.RequireLowercase;
    RequireUppercase = settings.RequireUppercase;
    RequireDigit = settings.RequireDigit;
  }

  /// <summary>
  /// Gets or sets the minimum number of characters in a password.
  /// </summary>
  public int RequiredLength { get; init; } = 6;
  /// <summary>
  /// Gets or sets the required number of unique characters in a password.
  /// </summary>
  public int RequiredUniqueChars { get; init; } = 1;
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include at least one non-alphanumeric character (e.g. !"/$%?&*_+±@£¢¤¬¦²³¼½¾).
  /// </summary>
  public bool RequireNonAlphanumeric { get; init; } = false;
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include a lowercase letter (a-z).
  /// </summary>
  public bool RequireLowercase { get; init; } = true;
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include an uppercase letter (A-Z).
  /// </summary>
  public bool RequireUppercase { get; init; } = true;
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords will need to include a digit (0-9).
  /// </summary>
  public bool RequireDigit { get; init; } = true;
}
