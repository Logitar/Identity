using Logitar.Identity.Core.Tokens;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

[Trait(Traits.Category, Categories.Integration)]
public class TokenBlacklistSqlServerIntegrationTests : IntegrationTests
{
  private readonly ITokenBlacklist _blacklist;

  public TokenBlacklistSqlServerIntegrationTests() : base(DatabaseProvider.SqlServer)
  {
    _blacklist = ServiceProvider.GetRequiredService<ITokenBlacklist>();
  }

  [Fact(DisplayName = "BlacklistAsync: it should blacklist the specified token identifiers with expiration.")]
  public async Task Given_TokenIdsAndExpiration_When_BlacklistAsync_Then_TokensAreBlacklisted()
  {
    BlacklistedTokenEntity entity = new(Guid.NewGuid().ToString());
    IdentityContext.TokenBlacklist.Add(entity);
    await IdentityContext.SaveChangesAsync();

    string[] tokenIds = [entity.TokenId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()];
    DateTime expiresOn = DateTime.Now.AddDays(7);
    await _blacklist.BlacklistAsync(tokenIds, expiresOn);

    BlacklistedTokenEntity[] entities = await IdentityContext.TokenBlacklist.AsNoTracking().ToArrayAsync();

    Assert.Equal(tokenIds.Length, entities.Length);
    foreach (string tokenId in tokenIds)
    {
      Assert.Contains(entities, e => e.TokenId == tokenId && (e.ExpiresOn - expiresOn.AsUniversalTime() < TimeSpan.FromSeconds(1)));
    }
  }

  [Fact(DisplayName = "GetBlacklistedAsync: it should return an empty collection when no blacklisted token matches.")]
  public async Task Given_NoMatchingToken_When_GetBlacklistedAsync_Then_EmptyReturned()
  {
    BlacklistedTokenEntity entity1 = new(Guid.NewGuid().ToString());
    BlacklistedTokenEntity entity2 = new(Guid.NewGuid().ToString())
    {
      ExpiresOn = DateTime.UtcNow.AddHours(-1)
    };
    IdentityContext.TokenBlacklist.AddRange(entity1, entity2);
    await IdentityContext.SaveChangesAsync();

    IReadOnlyCollection<string> tokenIds = await _blacklist.GetBlacklistedAsync([Guid.NewGuid().ToString(), entity2.TokenId]);

    Assert.Empty(tokenIds);
  }

  [Fact(DisplayName = "GetBlacklistedAsync: it should return the matching blacklisted tokens.")]
  public async Task Given_MatchingTokens_When_GetBlacklistedAsync_Then_EmptyReturned()
  {
    BlacklistedTokenEntity entity1 = new(Guid.NewGuid().ToString());
    BlacklistedTokenEntity entity2 = new(Guid.NewGuid().ToString())
    {
      ExpiresOn = DateTime.UtcNow.AddHours(-1)
    };
    BlacklistedTokenEntity entity3 = new(Guid.NewGuid().ToString())
    {
      ExpiresOn = DateTime.UtcNow.AddHours(1)
    };
    BlacklistedTokenEntity entity4 = new(Guid.NewGuid().ToString());
    IdentityContext.TokenBlacklist.AddRange(entity1, entity2, entity3, entity4);
    await IdentityContext.SaveChangesAsync();

    IReadOnlyCollection<string> tokenIds = await _blacklist.GetBlacklistedAsync([Guid.NewGuid().ToString(), entity2.TokenId, entity3.TokenId, entity4.TokenId]);

    Assert.Equal(2, tokenIds.Count);
    Assert.Contains(entity3.TokenId, tokenIds);
    Assert.Contains(entity4.TokenId, tokenIds);
  }

  [Fact(DisplayName = "PurgeAsync: it should not do anything where there are no expired tokens.")]
  public async Task Given_NoExpiredToken_When_PurgeAsync_Then_DoNothing()
  {
    BlacklistedTokenEntity entity = new(Guid.NewGuid().ToString())
    {
      ExpiresOn = DateTime.UtcNow.AddDays(7)
    };
    IdentityContext.TokenBlacklist.Add(entity);
    await IdentityContext.SaveChangesAsync();

    await _blacklist.PurgeAsync();

    Assert.NotNull(await IdentityContext.TokenBlacklist.AsNoTracking().SingleOrDefaultAsync());
  }

  [Fact(DisplayName = "PurgeAsync: it should purge the expired tokens.")]
  public async Task Given_ExpiredTokens_When_PurgeAsync_Then_Purged()
  {
    BlacklistedTokenEntity entity = new(Guid.NewGuid().ToString())
    {
      ExpiresOn = DateTime.UtcNow.AddDays(-7)
    };
    IdentityContext.TokenBlacklist.Add(entity);
    await IdentityContext.SaveChangesAsync();

    await _blacklist.PurgeAsync();

    Assert.Empty(await IdentityContext.TokenBlacklist.AsNoTracking().ToArrayAsync());
  }
}
