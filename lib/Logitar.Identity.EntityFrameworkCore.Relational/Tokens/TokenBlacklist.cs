using Logitar.Identity.Core.Tokens;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Tokens;

/// <summary>
/// Implements a token blacklist.
/// </summary>
public class TokenBlacklist : ITokenBlacklist
{
  /// <summary>
  /// Gets the Identity database context.
  /// </summary>
  protected virtual IdentityContext Context { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="TokenBlacklist"/> class.
  /// </summary>
  /// <param name="context">The Identity database context.</param>
  public TokenBlacklist(IdentityContext context)
  {
    Context = context;
  }

  /// <summary>
  /// Blacklists the specified list of token identifiers.
  /// </summary>
  /// <param name="tokenIds">The token identifiers to blacklist.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public virtual async Task BlacklistAsync(IEnumerable<string> tokenIds, CancellationToken cancellationToken)
  {
    await BlacklistAsync(tokenIds, expiresOn: null, cancellationToken);
  }
  /// <summary>
  /// Blacklists the specified list of token identifiers.
  /// </summary>
  /// <param name="tokenIds">The token identifiers to blacklist.</param>
  /// <param name="expiresOn">The expiration date and time of the token. If the kind is unspecified, the expiration will be treated as UTC.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public virtual async Task BlacklistAsync(IEnumerable<string> tokenIds, DateTime? expiresOn, CancellationToken cancellationToken)
  {
    expiresOn = expiresOn?.AsUniversalTime();

    Dictionary<string, BlacklistedTokenEntity> entities = await Context.TokenBlacklist
      .Where(x => tokenIds.Contains(x.TokenId))
      .ToDictionaryAsync(x => x.TokenId, x => x, cancellationToken);

    foreach (string tokenId in tokenIds)
    {
      if (!entities.TryGetValue(tokenId, out BlacklistedTokenEntity? entity))
      {
        entity = new(tokenId);
        entities[tokenId] = entity;

        Context.TokenBlacklist.Add(entity);
      }

      entity.ExpiresOn = expiresOn;
    }

    await Context.SaveChangesAsync(cancellationToken);
  }

  /// <summary>
  /// Returns the blacklisted token identifiers from the specified list of token identifiers.
  /// </summary>
  /// <param name="tokenIds">The list of token identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of blacklisted token identifiers.</returns>
  public virtual async Task<IReadOnlyCollection<string>> GetBlacklistedAsync(IEnumerable<string> tokenIds, CancellationToken cancellationToken)
  {
    DateTime now = DateTime.UtcNow;

    string[] blacklistedTokenIds = await Context.TokenBlacklist.AsNoTracking()
      .Where(x => tokenIds.Contains(x.TokenId) && (x.ExpiresOn == null || x.ExpiresOn > now))
      .Select(x => x.TokenId)
      .ToArrayAsync(cancellationToken);

    return blacklistedTokenIds;
  }

  /// <summary>
  /// Removes expired token identifiers from the blacklist.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public virtual async Task PurgeAsync(CancellationToken cancellationToken)
  {
    DateTime now = DateTime.UtcNow;

    BlacklistedTokenEntity[] expiredEntities = await Context.TokenBlacklist
      .Where(x => x.ExpiresOn != null && x.ExpiresOn <= now)
      .ToArrayAsync(cancellationToken);

    Context.TokenBlacklist.RemoveRange(expiredEntities);
    await Context.SaveChangesAsync(cancellationToken);
  }
}
