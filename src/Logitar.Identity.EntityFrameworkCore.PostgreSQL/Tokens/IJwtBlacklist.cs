namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Tokens;

/// <summary>
/// Defines the methods of a blacklist of JSON Web Tokens.
/// </summary>
internal interface IJwtBlacklist
{
  /// <summary>
  /// Blacklists the specified token identifiers, until the specified date and time.
  /// </summary>
  /// <param name="ids">The list of identifiers to blacklist.</param>
  /// <param name="expiresOn">The date and time when the blacklisting will expire.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task BlacklistAsync(IEnumerable<Guid> ids, DateTime? expiresOn = null, CancellationToken cancellationToken = default);
  /// <summary>
  /// Returns the blacklisted token identifiers in the specified list of token identifiers.
  /// </summary>
  /// <param name="ids">The list of token identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of blacklisted token identifiers.</returns>
  Task<IEnumerable<Guid>> GetBlacklistedAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}
