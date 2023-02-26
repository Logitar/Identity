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
}
