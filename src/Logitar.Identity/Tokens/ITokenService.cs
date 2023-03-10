namespace Logitar.Identity.Tokens;

/// <summary>
/// Exposes methods to manage tokens in the identity system.
/// </summary>
public interface ITokenService
{
  /// <summary>
  /// Creates a new token.
  /// </summary>
  /// <param name="input">The input creation arguments.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The newly created token string.</returns>
  Task<string> CreateAsync(CreateTokenInput input, CancellationToken cancellationToken = default);
  /// <summary>
  /// Validates a security token.
  /// </summary>
  /// <param name="input">The input validation arguments.</param>
  /// <param name="consume">If true, the token will be consumed. A consumed token cannot be used again.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The validated token claims.</returns>
  Task<IEnumerable<Claim>> ValidateAsync(ValidateTokenInput input, bool consume = false, CancellationToken cancellationToken = default);
}
