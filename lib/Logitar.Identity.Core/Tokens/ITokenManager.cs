namespace Logitar.Identity.Core.Tokens;

/// <summary>
/// Defines methods to manage tokens.
/// </summary>
public interface ITokenManager
{
  /// <summary>
  /// Creates a token for the specified subject, using the specified signing secret.
  /// </summary>
  /// <param name="subject">The subject of the token.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The created token.</returns>
  Task<CreatedToken> CreateAsync(ClaimsIdentity subject, string secret, CancellationToken cancellationToken = default);
  /// <summary>
  /// Creates a token for the specified subject, using the specified signing secret and creation options.
  /// </summary>
  /// <param name="subject">The subject of the token.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="options">The creation options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The created token.</returns>
  Task<CreatedToken> CreateAsync(ClaimsIdentity subject, string secret, CreateTokenOptions? options, CancellationToken cancellationToken = default);
  /// <summary>
  /// Creates a token with the specified parameters.
  /// </summary>
  /// <param name="parameters">The creation parameters.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The created token.</returns>
  Task<CreatedToken> CreateAsync(CreateTokenParameters parameters, CancellationToken cancellationToken = default);

  /// <summary>
  /// Validates a token using the specified signing secret.
  /// </summary>
  /// <param name="token">The token to validate.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The validated token.</returns>
  Task<ValidatedToken> ValidateAsync(string token, string secret, CancellationToken cancellationToken = default);
  /// <summary>
  /// Validates a token using the specified signing secret and validation options.
  /// </summary>
  /// <param name="token">The token to validate.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="options">The validation options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The validated token.</returns>
  Task<ValidatedToken> ValidateAsync(string token, string secret, ValidateTokenOptions? options, CancellationToken cancellationToken = default);
  /// <summary>
  /// Validates a token with the specified parameters.
  /// </summary>
  /// <param name="parameters">The validation parameters.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The validated token.</returns>
  Task<ValidatedToken> ValidateAsync(ValidateTokenParameters parameters, CancellationToken cancellationToken = default);
}
