namespace Logitar.Identity.Domain.Tokens;

/// <summary>
/// Defines a token blacklist.
/// </summary>
public interface ITokenBlacklist
{
  /// <summary>
  /// Blacklists the specified list of token identifiers.
  /// </summary>
  /// <param name="tokenIds">The token identifiers to blacklist.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task BlacklistAsync(IEnumerable<string> tokenIds, CancellationToken cancellationToken = default);
  /// <summary>
  /// Blacklists the specified list of token identifiers.
  /// </summary>
  /// <param name="tokenIds">The token identifiers to blacklist.</param>
  /// <param name="expiresOn">The expiration date and time of the token.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task BlacklistAsync(IEnumerable<string> tokenIds, DateTime? expiresOn, CancellationToken cancellationToken = default);
  /// <summary>
  /// Returns the blacklisted token identifiers from the specified list of token identifiers.
  /// </summary>
  /// <param name="tokenIds">The list of token identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of blacklisted token identifiers.</returns>
  Task<IEnumerable<string>> GetBlacklistedAsync(IEnumerable<string> tokenIds, CancellationToken cancellationToken = default);
  /// <summary>
  /// Removes expired token identifiers from the blacklist.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task PurgeAsync(CancellationToken cancellationToken = default);
}
