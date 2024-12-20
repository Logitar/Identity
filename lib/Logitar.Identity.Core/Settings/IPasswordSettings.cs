namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate passwords.
/// </summary>
public interface IPasswordSettings // TODO(fpion): move to Contracts
{
  /// <summary>
  /// Gets the required length (number of characters) of passwords.
  /// </summary>
  int RequiredLength { get; }
  /// <summary>
  /// Gets the required number of unique characters in passwords.
  /// </summary>
  int RequiredUniqueChars { get; }
  /// <summary>
  /// Gets a value indicating whether or not passwords require a non-alphanumeric character.
  /// </summary>
  bool RequireNonAlphanumeric { get; }
  /// <summary>
  /// Gets a value indicating whether or not passwords require a lowercase character.
  /// </summary>
  bool RequireLowercase { get; }
  /// <summary>
  /// Gets a value indicating whether or not passwords require an uppercase character.
  /// </summary>
  bool RequireUppercase { get; }
  /// <summary>
  /// Gets a value indicating whether or not passwords require a digit character.
  /// </summary>
  bool RequireDigit { get; }
  /// <summary>
  /// Gets the key of the password hashing strategy.
  /// </summary>
  string HashingStrategy { get; }
}
