using Logitar.Identity.Contracts.Settings;

namespace Logitar.Identity.Domain.Settings;

/// <summary>
/// The settings used to validate passwords.
/// </summary>
public record PasswordSettings : IPasswordSettings
{
  /// <summary>
  /// Gets or sets the required length (number of characters) of passwords.
  /// </summary>
  public int RequiredLength { get; set; } = 8;
  /// <summary>
  /// Gets or sets the required number of unique characters in passwords.
  /// </summary>
  public int RequiredUniqueChars { get; set; } = 8;
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords require a non-alphanumeric character.
  /// </summary>
  public bool RequireNonAlphanumeric { get; set; } = true;
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords require a lowercase character.
  /// </summary>
  public bool RequireLowercase { get; set; } = true;
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords require an uppercase character.
  /// </summary>
  public bool RequireUppercase { get; set; } = true;
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords require a digit character.
  /// </summary>
  public bool RequireDigit { get; set; } = true;
  /// <summary>
  /// Gets or sets the key of the password hashing strategy.
  /// </summary>
  public string HashingStrategy { get; set; } = "PBKDF2";
}
