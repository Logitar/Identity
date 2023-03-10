using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Tokens;

/// <summary>
/// Implements the methods of a blacklist of JSON Web Tokens.
/// </summary>
internal class JwtBlacklist : IJwtBlacklist
{
  /// <summary>
  /// The identity context.
  /// </summary>
  private readonly IdentityContext _context;

  /// <summary>
  /// Initializes a new instance of the <see cref="JwtBlacklist"/> class using the specified arguments.
  /// </summary>
  /// <param name="context">The identity context.</param>
  public JwtBlacklist(IdentityContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Blacklists the specified token identifiers, until the specified date and time.
  /// </summary>
  /// <param name="ids">The list of identifiers to blacklist.</param>
  /// <param name="expiresOn">The date and time when the blacklisting will expire.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public async Task BlacklistAsync(IEnumerable<Guid> ids, DateTime? expiresOn, CancellationToken cancellationToken = default)
  {
    _context.JwtBlacklist.AddRange(ids.Select(id => new BlacklistedJwtEntity(id, expiresOn)));

    await _context.SaveChangesAsync(cancellationToken);
  }

  /// <summary>
  /// Returns the blacklisted token identifiers in the specified list of token identifiers.
  /// </summary>
  /// <param name="ids">The list of token identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of blacklisted token identifiers.</returns>
  public async Task<IEnumerable<Guid>> GetBlacklistedAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
  {
    return await _context.JwtBlacklist.AsNoTracking()
      .Where(x => ids.Contains(x.Id) && (x.ExpiresOn == null || x.ExpiresOn > DateTime.UtcNow))
      .Select(x => x.Id)
      .ToArrayAsync(cancellationToken);
  }
}
