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

  /// <summary>
  /// Returns a value indicating whether or not the specified key matches the specified session.
  /// </summary>
  /// <param name="session">The session to compare.</param>
  /// <param name="key">The key to match.</param>
  /// <returns>True if the key matches the session's salted and hashed key.</returns>
  public bool IsMatch(SessionAggregate session, byte[] key)
  {
    if (!session.IsPersistent)
    {
      return false;
    }

    Pbkdf2 pbkdf2 = Pbkdf2.Parse(session.KeyHash!);

    return pbkdf2.IsMatch(Convert.ToBase64String(key));
  }
}
