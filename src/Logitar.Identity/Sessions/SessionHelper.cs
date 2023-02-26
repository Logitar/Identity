using System.Security.Cryptography;

namespace Logitar.Identity.Sessions;

/// <summary>
/// Implements methods to help managing user sessions.
/// </summary>
internal class SessionHelper : ISessionHelper
{
  /// <summary>
  /// Generates a session key.
  /// </summary>
  /// <param name="key">The key bytes.</param>
  /// <returns>The salted and hashed key.</returns>
  public string GenerateKey(out byte[] key)
  {
    key = RandomNumberGenerator.GetBytes(32);

    return new Pbkdf2(Convert.ToBase64String(key)).ToString();
  }
}
