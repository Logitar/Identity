using FluentValidation;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Settings;

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
  /// Creates a password from the specified string.
  /// </summary>
  /// <param name="password">The password string.</param>
  /// <returns>The password instance.</returns>
  public virtual Password Create(string password)
  {
    return Create(password, validate: true);
  }
  /// <summary>
  /// Creates a password from the specified string, validating it against password rules if specified.
  /// </summary>
  /// <param name="password">The password string.</param>
  /// <param name="validate">A value indicating whether or not to validate the password against password rules. Validation should not be deactivated when creating user passwords.</param>
  /// <returns>The password instance.</returns>
  public virtual Password Create(string password, bool validate)
  {
    IPasswordSettings passwordSettings = SettingsResolver.Resolve().Password;

    if (validate)
    {
      new PasswordValidator(passwordSettings).ValidateAndThrow(password);
    }

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
  /// Generates a new password of the specified length.
  /// </summary>
  /// <param name="length">The length of the password, in bytes.</param>
  /// <param name="password">The password bytes.</param>
  /// <returns>The password instance.</returns>
  public virtual Password Generate(int length, out byte[] password)
  {
    password = RandomNumberGenerator.GetBytes(length);
    string passwordString = Convert.ToBase64String(password);
    return Create(passwordString, validate: false);
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
