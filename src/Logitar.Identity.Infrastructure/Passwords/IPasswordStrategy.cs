using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Infrastructure.Passwords;

/// <summary>
/// Defines a password strategy.
/// </summary>
public interface IPasswordStrategy
{
  /// <summary>
  /// Gets the unique key of this strategy.
  /// </summary>
  string Key { get; }

  /// <summary>
  /// Creates a password instance from the specified raw password string.
  /// </summary>
  /// <param name="password">The raw password string.</param>
  /// <returns>The password instance.</returns>
  Password Create(string password);
  /// <summary>
  /// Decodes the specified password string into a password instance.
  /// </summary>
  /// <param name="password">The encoded password string.</param>
  /// <returns>The password instance.</returns>
  Password Decode(string password);
}
