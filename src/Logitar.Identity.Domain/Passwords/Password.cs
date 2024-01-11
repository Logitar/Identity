namespace Logitar.Identity.Domain.Passwords;

/// <summary>
/// Represents a password or a secret.
/// </summary>
public abstract record Password
{
  /// <summary>
  /// The separator character to use between password components.
  /// </summary>
  public const char Separator = ':';

  /// <summary>
  /// Encodes the password to a string representation.
  /// </summary>
  /// <returns>The string representation of the password.</returns>
  public abstract string Encode();
  /// <summary>
  /// Returns a value indicating whether or not the specified password matches the current password.
  /// </summary>
  /// <param name="password">The password to match.</param>
  /// <returns>True if the password matches the current password, or false otherwise.</returns>
  public abstract bool IsMatch(string password);
}
