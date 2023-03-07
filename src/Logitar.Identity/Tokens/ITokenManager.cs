using System.Security.Claims;

namespace Logitar.Identity.Tokens;

/// <summary>
/// Exposes methods to manage security tokens in the identity system.
/// </summary>
public interface ITokenManager
{
  /// <summary>
  /// Creates a security token using the specified arguments.
  /// </summary>
  /// <param name="subject">The subject identity represented by the security token.</param>
  /// <param name="secret">The secret used to sign the token.</param>
  /// <param name="algorithm">The algorithm used to sign the token.</param>
  /// <param name="expires">The date and time when the token expires.</param>
  /// <param name="audience">The audience of the token.</param>
  /// <param name="issuer">The issuer of the token.</param>
  /// <returns>The token string.</returns>
  string Create(ClaimsIdentity subject, string? secret = null, string? algorithm = null, DateTime? expires = null, string? audience = null, string? issuer = null);
}
