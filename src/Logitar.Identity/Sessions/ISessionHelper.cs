namespace Logitar.Identity.Sessions;

/// <summary>
/// Defines methods to help managing sessions.
/// </summary>
internal interface ISessionHelper
{
  /// <summary>
  /// Generates a session key.
  /// </summary>
  /// <param name="key">The key bytes.</param>
  /// <returns>The salted and hashed key.</returns>
  string GenerateKey(out byte[] key);

  /// <summary>
  /// Returns a value indicating whether or not the specified key matches the specified session.
  /// </summary>
  /// <param name="session">The session to compare.</param>
  /// <param name="key">The key to match.</param>
  /// <returns>True if the key matches the session's salted and hashed key.</returns>
  bool IsMatch(SessionAggregate session, byte[] key);
}
