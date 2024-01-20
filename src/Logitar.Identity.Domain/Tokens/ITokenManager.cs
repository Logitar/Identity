namespace Logitar.Identity.Domain.Tokens;

/// <summary>
/// Defines methods to manage tokens.
/// </summary>
public interface ITokenManager
{
  /// <summary>
  /// Creates a token for the specified subject, using the specified signing secret and creation options.
  /// </summary>
  /// <param name="subject">The subject of the token.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="options">The creation options.</param>
  /// <returns>The created token.</returns>
  CreatedToken Create(ClaimsIdentity subject, string secret, CreateTokenOptions? options = null);
  /// <summary>
  /// Creates a token with the specified parameters.
  /// </summary>
  /// <param name="parameters">The creation parameters.</param>
  /// <returns>The created token.</returns>
  CreatedToken Create(CreateTokenParameters parameters);
}
