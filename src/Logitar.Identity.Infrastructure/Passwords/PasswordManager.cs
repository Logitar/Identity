using FluentValidation;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Settings;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Infrastructure.Passwords;

/// <summary>
/// Implements methods to manage passwords.
/// </summary>
public class PasswordManager : IPasswordManager
{
  /// <summary>
  /// Gets the user settings resolver.
  /// </summary>
  protected virtual IUserSettingsResolver SettingsResolver { get; }
  /// <summary>
  /// Gets the registered password strategies.
  /// </summary>
  protected virtual Dictionary<string, IPasswordStrategy> Strategies { get; } = [];

  /// <summary>
  /// Initializes a new instance of the <see cref="PasswordManager"/> class.
  /// </summary>
  /// <param name="settingsResolver">The user settings resolver.</param>
  /// <param name="strategies">The registered password strategies.</param>
  public PasswordManager(IUserSettingsResolver settingsResolver, IEnumerable<IPasswordStrategy> strategies)
  {
    SettingsResolver = settingsResolver;

    foreach (IPasswordStrategy strategy in strategies)
    {
      Strategies[strategy.Key] = strategy;
    }
  }

  /// <summary>
  /// Creates a password from the specified string. This method should not be used to create strong user passwords.
  /// </summary>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  public virtual Password Create(string password)
  {
    IPasswordSettings passwordSettings = SettingsResolver.Resolve().Password;
    return GetStrategy(passwordSettings.HashingStrategy).Create(password);
  }

  /// <summary>
  /// Decodes a password from the encoded string.
  /// </summary>
  /// <param name="password">The encoded password.</param>
  /// <returns>The password instance.</returns>
  public virtual Password Decode(string password)
  {
    string key = password.Split(Password.Separator).First();
    return GetStrategy(key).Decode(password);
  }

  /// <summary>
  /// Generates a password from a cryptographically strong random sequence of characters, selecting characters randomly in the following character set:
  /// <br />!"#$%&amp;'()*+,-./0123456789:;&lt;=&gt;?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~
  /// </summary>
  /// <param name="length">The length of the password, in number of characters.</param>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  public Password Generate(int length, out string password)
  {
    password = RandomStringGenerator.GetString(length);
    return Create(password);
  }
  /// <summary>
  /// Generates a password from a cryptographically strong random sequence of bytes selected from the specified character set.
  /// </summary>
  /// <param name="characters">The list of characters to pick from.</param>
  /// <param name="length">The length of the password, in number of characters.</param>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  public Password Generate(string characters, int length, out string password)
  {
    password = RandomStringGenerator.GetString(characters, length);
    return Create(password);
  }

  /// <summary>
  /// Generates a password from the specified number of bytes, then converts it to Base64 and creates a password from the generated string.
  /// </summary>
  /// <param name="length">The length of the password, in number of bytes.</param>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  public Password GenerateBase64(int length, out string password)
  {
    password = RandomStringGenerator.GetBase64String(length, out _);
    return Create(password);
  }

  /// <summary>
  /// Validates the specified password string.
  /// </summary>
  /// <param name="password">The password string.</param>
  public void Validate(string password)
  {
    IPasswordSettings passwordSettings = SettingsResolver.Resolve().Password;
    new PasswordValidator(passwordSettings).ValidateAndThrow(password);
  }

  /// <summary>
  /// Validates the specified password string, then creates a password if it is valid, or throws an exception otherwise.
  /// </summary>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  /// <exception cref="ValidationException">The password is too weak.</exception>
  public Password ValidateAndCreate(string password)
  {
    Validate(password);
    return Create(password);
  }

  /// <summary>
  /// Returns the password strategy identified by the specified key.
  /// </summary>
  /// <param name="key">The password strategy key.</param>
  /// <returns></returns>
  /// <exception cref="PasswordStrategyNotSupportedException">No password strategy has been registered for the specified key.</exception>
  protected virtual IPasswordStrategy GetStrategy(string key)
  {
    return Strategies.TryGetValue(key, out IPasswordStrategy? strategy)
      ? strategy
      : throw new PasswordStrategyNotSupportedException(key);
  }
}
