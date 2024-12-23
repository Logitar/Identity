using Logitar.Identity.Contracts.Settings;

namespace Logitar.Identity.Core.Passwords;

/// <summary>
/// Defines methods to manage passwords.
/// </summary>
public interface IPasswordManager
{
  /// <summary>
  /// Creates a password from the specified string. This method should not be used to create strong user passwords.
  /// </summary>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  Password Create(string password);
  /// <summary>
  /// Creates a password from the specified string. This method should not be used to create strong user passwords.
  /// </summary>
  /// <param name="password">The password string.</param>
  /// <param name="passwordSettings">The password settings.</param>
  /// <returns>The password instance.</returns>
  Password Create(string password, IPasswordSettings? passwordSettings);

  /// <summary>
  /// Decodes a password from the encoded string.
  /// </summary>
  /// <param name="password">The encoded password.</param>
  /// <returns>The password instance.</returns>
  Password Decode(string password);

  /// <summary>
  /// Generates a password from a cryptographically strong random sequence of characters, selecting characters randomly in the following character set:
  /// <br />!"#$%&amp;'()*+,-./0123456789:;&lt;=&gt;?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~
  /// </summary>
  /// <param name="length">The length of the password, in number of characters.</param>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  Password Generate(int length, out string password);
  /// <summary>
  /// Generates a password from a cryptographically strong random sequence of bytes selected from the specified character set.
  /// </summary>
  /// <param name="characters">The list of characters to pick from.</param>
  /// <param name="length">The length of the password, in number of characters.</param>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  Password Generate(string characters, int length, out string password);

  /// <summary>
  /// Generates a password from the specified number of bytes, then converts it to Base64 and creates a password from the generated string.
  /// </summary>
  /// <param name="length">The length of the password, in number of bytes.</param>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  Password GenerateBase64(int length, out string password);

  /// <summary>
  /// Validates the specified password string.
  /// </summary>
  /// <param name="password">The password string.</param>
  void Validate(string password);
  /// <summary>
  /// Validates the specified password string.
  /// </summary>
  /// <param name="password">The password string.</param>
  /// <param name="passwordSettings">The password settings.</param>
  void Validate(string password, IPasswordSettings? passwordSettings);

  /// <summary>
  /// Validates the specified password string, then creates a password if it is valid, or throws an exception otherwise.
  /// </summary>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  /// <exception cref="FluentValidation.ValidationException">The password is too weak.</exception>
  Password ValidateAndCreate(string password);
  /// <summary>
  /// Validates the specified password string, then creates a password if it is valid, or throws an exception otherwise.
  /// </summary>
  /// <param name="password">The password string.</param>
  /// <param name="passwordSettings">The password settings.</param>
  /// <returns>The password instance.</returns>
  /// <exception cref="FluentValidation.ValidationException">The password is too weak.</exception>
  Password ValidateAndCreate(string password, IPasswordSettings? passwordSettings);
}
