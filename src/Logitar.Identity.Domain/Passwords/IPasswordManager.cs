namespace Logitar.Identity.Domain.Passwords;

/// <summary>
/// Defines methods to manage passwords.
/// </summary>
public interface IPasswordManager
{
  /// <summary>
  /// Creates a password from the specified string.
  /// </summary>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  Password Create(string password);
  /// <summary>
  /// Decodes a password from the encoded string.
  /// </summary>
  /// <param name="password">The encoded password.</param>
  /// <returns>The password instance.</returns>
  Password Decode(string password);
  /// <summary>
  /// Generates a new password of the specified length.
  /// </summary>
  /// <param name="length">The length of the password, in bytes.</param>
  /// <param name="password">The password bytes.</param>
  /// <returns>The password instance.</returns>
  Password Generate(int length, out byte[] password);
}
