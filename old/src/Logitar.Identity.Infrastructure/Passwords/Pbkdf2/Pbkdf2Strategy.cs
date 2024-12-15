using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Infrastructure.Passwords.Pbkdf2;

/// <summary>
/// Implements a password strategy using the Password-Based Key Derivation Function 2 (PBKDF2).
/// </summary>
public class Pbkdf2Strategy : IPasswordStrategy
{
  private readonly Pbkdf2Settings _settings;

  /// <summary>
  /// Gets the unique key of this strategy.
  /// </summary>
  public string Key => Pbkdf2Password.Key;

  /// <summary>
  /// Initializes a new instance of the <see cref="Pbkdf2Strategy"/> class.
  /// </summary>
  /// <param name="settings">The PBKDF2 settings.</param>
  public Pbkdf2Strategy(Pbkdf2Settings settings)
  {
    _settings = settings;
  }

  /// <summary>
  /// Creates a password instance from the specified raw password string.
  /// </summary>
  /// <param name="password">The raw password string.</param>
  /// <returns>The password instance.</returns>
  public Password Create(string password)
  {
    return new Pbkdf2Password(password, _settings.Algorithm, _settings.Iterations, _settings.SaltLength, _settings.HashLength);
  }

  /// <summary>
  /// Decodes the specified password string into a password instance.
  /// </summary>
  /// <param name="password">The encoded password string.</param>
  /// <returns>The password instance.</returns>
  public Password Decode(string password)
  {
    return Pbkdf2Password.Decode(password);
  }
}
