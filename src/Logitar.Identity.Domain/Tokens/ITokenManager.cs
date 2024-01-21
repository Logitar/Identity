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

  /// <summary>
  /// Validates a token using the specified signing secret and validation options.
  /// </summary>
  /// <param name="token">The token to validate.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="options">The validation options.</param>
  /// <returns>The validated token.</returns>
  ValidatedToken Validate(string token, string secret, ValidateTokenOptions? options = null);
  /// <summary>
  /// Validates a token with the specified parameters.
  /// </summary>
  /// <param name="parameters">The validation parameters.</param>
  /// <returns>The validated token.</returns>
  ValidatedToken Validate(ValidateTokenParameters parameters);
}
