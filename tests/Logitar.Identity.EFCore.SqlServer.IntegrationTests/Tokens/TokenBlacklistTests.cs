using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Domain.Tokens;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Tokens;

[Trait(Traits.Category, Categories.Integration)]
public class TokenBlacklistTests : IAsyncLifetime
{
  private readonly EventContext _eventContext;
  private readonly IdentityContext _identityContext;
  private readonly IServiceProvider _serviceProvider;
  private readonly ITokenBlacklist _tokenBlacklist;

  public TokenBlacklistTests()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .Build();

    string connectionString = (configuration.GetValue<string>("SQLCONNSTR_Identity") ?? string.Empty)
      .Replace("{Database}", nameof(TokenBlacklistTests));

    _serviceProvider = new ServiceCollection()
      .AddSingleton(configuration)
      .AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString)
      .BuildServiceProvider();

    _eventContext = _serviceProvider.GetRequiredService<EventContext>();
    _identityContext = _serviceProvider.GetRequiredService<IdentityContext>();
    _tokenBlacklist = _serviceProvider.GetRequiredService<ITokenBlacklist>();
  }

  public async Task InitializeAsync()
  {
    await _eventContext.Database.MigrateAsync();
    await _identityContext.Database.MigrateAsync();

    TableId[] tables = [IdentityDb.TokenBlacklist.Table];
    foreach (TableId table in tables)
    {
      ICommand command = SqlServerDeleteBuilder.From(table).Build();
      await _identityContext.Database.ExecuteSqlRawAsync(command.Text, command.Parameters.ToArray());
    }
  }

  [Fact(DisplayName = "BlacklistAsync: it should blacklist identifiers with expiration.")]
  public async Task BlacklistAsync_it_should_blacklist_identifiers_with_expiration()
  {
    string duplicate = Guid.NewGuid().ToString();
    string[] tokenIds = [Guid.NewGuid().ToString(), duplicate, duplicate];
    DateTime expiresOn = DateTime.UtcNow.AddHours(1);

    await _tokenBlacklist.BlacklistAsync(tokenIds, expiresOn);
    Dictionary<string, BlacklistedTokenEntity> entities = await _identityContext.TokenBlacklist.AsNoTracking()
      .ToDictionaryAsync(x => x.TokenId, x => x);
    Assert.Equal(2, entities.Count);
    foreach (string tokenId in tokenIds)
    {
      Assert.True(entities.ContainsKey(tokenId));
      Assert.Equal(expiresOn, entities[tokenId].ExpiresOn);
    }
  }

  [Fact(DisplayName = "BlacklistAsync: it should blacklist identifiers without expiration.")]
  public async Task BlacklistAsync_it_should_blacklist_identifiers_without_expiration()
  {
    string duplicate = Guid.NewGuid().ToString();
    string[] tokenIds = [Guid.NewGuid().ToString(), duplicate, duplicate];
    Dictionary<string, BlacklistedTokenEntity> entities;

    await _tokenBlacklist.BlacklistAsync(tokenIds);
    entities = await _identityContext.TokenBlacklist.AsNoTracking().ToDictionaryAsync(x => x.TokenId, x => x);
    foreach (string tokenId in tokenIds)
    {
      Assert.True(entities.ContainsKey(tokenId));
      Assert.Null(entities[tokenId].ExpiresOn);
    }

    await _tokenBlacklist.BlacklistAsync(tokenIds, expiresOn: null);
    entities = await _identityContext.TokenBlacklist.AsNoTracking().ToDictionaryAsync(x => x.TokenId, x => x);
    foreach (string tokenId in tokenIds)
    {
      Assert.True(entities.ContainsKey(tokenId));
      Assert.Null(entities[tokenId].ExpiresOn);
    }
  }

  [Fact(DisplayName = "GetBlacklistedAsync: it should return empty when no ID is blacklisted.")]
  public async Task GetBlacklistedAsync_it_should_return_empty_when_no_Id_is_blacklisted()
  {
    string[] tokenIds = [Guid.NewGuid().ToString(), Guid.NewGuid().ToString()];
    IEnumerable<string> blacklistedIds = await _tokenBlacklist.GetBlacklistedAsync(tokenIds);
    Assert.Empty(blacklistedIds);
  }

  [Fact(DisplayName = "GetBlacklistedAsync: it should return only the blacklisted IDs.")]
  public async Task GetBlacklistedAsync_it_should_return_only_the_blacklisted_Ids()
  {
    string blacklistedId = Guid.NewGuid().ToString();
    string otherId = Guid.NewGuid().ToString();

    BlacklistedTokenEntity entity = new(blacklistedId);
    _identityContext.TokenBlacklist.Add(entity);
    await _identityContext.SaveChangesAsync();

    string[] tokenIds = [blacklistedId, otherId];
    IEnumerable<string> blacklistedIds = await _tokenBlacklist.GetBlacklistedAsync(tokenIds);
    Assert.Equal([blacklistedId], blacklistedIds);
  }

  [Fact(DisplayName = "PurgeAsync: it should remove only expired items.")]
  public async Task PurgeAsync_it_should_remove_only_expired_items()
  {
    BlacklistedTokenEntity expired = new("expired")
    {
      ExpiresOn = DateTime.Now.AddDays(-1)
    };
    BlacklistedTokenEntity notExpired = new("notExpired")
    {
      ExpiresOn = DateTime.Now.AddDays(1)
    };
    BlacklistedTokenEntity noExpiration = new("noExpiration");
    _identityContext.TokenBlacklist.AddRange(expired, notExpired, noExpiration);
    await _identityContext.SaveChangesAsync();

    await _tokenBlacklist.PurgeAsync();

    HashSet<string> entities = [.. (await _identityContext.TokenBlacklist.AsNoTracking().Select(x => x.TokenId).ToArrayAsync())];
    Assert.Equal(2, entities.Count);
    Assert.Contains(notExpired.TokenId, entities);
    Assert.Contains(noExpiration.TokenId, entities);
  }

  public Task DisposeAsync() => Task.CompletedTask;
}
