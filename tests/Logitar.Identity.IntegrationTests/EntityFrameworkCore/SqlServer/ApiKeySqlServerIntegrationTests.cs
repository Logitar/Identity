using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.Infrastructure.Passwords.Pbkdf2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

[Trait(Traits.Category, Categories.Integration)]
public class ApiKeySqlServerIntegrationTests : IntegrationTests
{
  private const string SecretString = "P@s$W0rD";

  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IRoleRepository _roleRepository;

  private readonly ActorId _actorId = ActorId.NewId();
  private readonly Password _secret;

  public ApiKeySqlServerIntegrationTests() : base(DatabaseProvider.SqlServer)
  {
    _apiKeyRepository = ServiceProvider.GetRequiredService<IApiKeyRepository>();
    _roleRepository = ServiceProvider.GetRequiredService<IRoleRepository>();

    Pbkdf2Settings pbkdf2 = new();
    _secret = new Pbkdf2Password(SecretString, pbkdf2.Algorithm, pbkdf2.Iterations, pbkdf2.SaltLength, pbkdf2.HashLength);
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a deletion status.")]
  [InlineData(null)]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_Deleted_When_LoadAsync_Then_CorrectResult(bool? isDeleted)
  {
    ApiKey apiKey1 = new(new DisplayName("API Key #1"), _secret);
    ApiKey apiKey2 = new(new DisplayName("API Key #2"), _secret);
    apiKey2.Delete();
    ApiKey apiKey3 = new(new DisplayName("API Key #3"), _secret);
    if (isDeleted == true)
    {
      apiKey3.Delete();
    }
    if (isDeleted.HasValue)
    {
      await _apiKeyRepository.SaveAsync([apiKey1, apiKey2, apiKey3]);
    }

    IReadOnlyCollection<ApiKey> apiKeys = await _apiKeyRepository.LoadAsync(isDeleted);

    if (isDeleted.HasValue)
    {
      Assert.Equal(2, apiKeys.Count);
      Assert.Contains(apiKey3, apiKeys);
      Assert.Contains(isDeleted.Value ? apiKey2 : apiKey1, apiKeys);
    }
    else
    {
      Assert.Empty(apiKeys);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given IDs and a deletion status.")]
  [InlineData(null)]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_IdsDeleted_When_LoadAsync_Then_CorrectResult(bool? isDeleted)
  {
    ApiKey apiKey1 = new(new DisplayName("API Key #1"), _secret);
    ApiKey apiKey2 = new(new DisplayName("API Key #2"), _secret);
    apiKey2.Delete();
    ApiKey apiKey3 = new(new DisplayName("API Key #3"), _secret);
    if (isDeleted == true)
    {
      apiKey3.Delete();
    }
    ApiKey apiKey4 = new(new DisplayName("API Key #4"), _secret);
    if (isDeleted == true)
    {
      apiKey4.Delete();
    }
    if (isDeleted.HasValue)
    {
      await _apiKeyRepository.SaveAsync([apiKey1, apiKey2, apiKey3, apiKey4]);
    }

    IReadOnlyCollection<ApiKey> apiKeys = await _apiKeyRepository.LoadAsync([apiKey1.Id, apiKey2.Id, apiKey3.Id, ApiKeyId.NewId()], isDeleted);

    if (isDeleted.HasValue)
    {
      Assert.Equal(2, apiKeys.Count);
      Assert.Contains(apiKey3, apiKeys);
      Assert.Contains(isDeleted.Value ? apiKey2 : apiKey1, apiKeys);
    }
    else
    {
      Assert.Empty(apiKeys);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given an ID, a version and a deletion status.")]
  [InlineData(false, null, false)]
  [InlineData(true, null, false)]
  [InlineData(true, false, false)]
  [InlineData(true, true, true)]
  public async Task Given_IdVersionDeleted_When_LoadAsync_Then_CorrectResult(bool found, bool? isDeleted, bool withVersion)
  {
    ApiKey apiKey = new(new DisplayName("Test"), _secret);
    if (isDeleted == true)
    {
      apiKey.Delete();
    }
    if (found)
    {
      await _apiKeyRepository.SaveAsync(apiKey);
    }

    long? version = withVersion ? 1 : null;
    ApiKey? result = await _apiKeyRepository.LoadAsync(apiKey.Id, version, isDeleted);
    if (found)
    {
      Assert.NotNull(result);
      if (withVersion)
      {
        Assert.Equal(version, result.Version);
        Assert.False(result.IsDeleted);
      }
      else
      {
        Assert.Equal(apiKey.Version, result.Version);
        Assert.Equal(isDeleted ?? false, result.IsDeleted);
      }
    }
    else
    {
      Assert.Null(result);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a role.")]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_Role_When_LoadAsync_Then_CorrectResults(bool found)
  {
    Role role = new(new UniqueName(new UniqueNameSettings(), "admin"));
    await _roleRepository.SaveAsync(role);

    ApiKey apiKey = new(new DisplayName("Test"), _secret);
    if (found)
    {
      apiKey.AddRole(role);
    }
    await _apiKeyRepository.SaveAsync(apiKey);

    IReadOnlyCollection<ApiKey> result = await _apiKeyRepository.LoadAsync(role);

    if (found)
    {
      Assert.Equal(apiKey, Assert.Single(result));
    }
    else
    {
      Assert.Empty(result);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a tenant ID.")]
  [InlineData(false, null)]
  [InlineData(true, null)]
  [InlineData(true, "3812eedf-7976-4784-92cd-2ae34ab4414d")]
  public async Task Given_TenantId_When_LoadAsync_Then_CorrectResults(bool found, string? tenantIdValue)
  {
    ApiKey apiKey1 = new(new DisplayName("Without Tenant"), _secret);
    TenantId tenantId = tenantIdValue == null ? TenantId.NewId() : new(tenantIdValue);
    ApiKey apiKey2 = new(new DisplayName("With Tenant"), _secret, actorId: null, ApiKeyId.NewId(tenantId));
    if (found)
    {
      await _apiKeyRepository.SaveAsync([apiKey1, apiKey2]);
    }

    IReadOnlyCollection<ApiKey> apiKeys = await _apiKeyRepository.LoadAsync(tenantIdValue == null ? null : tenantId);

    if (found)
    {
      Assert.Equal(tenantIdValue == null ? apiKey1 : apiKey2, Assert.Single(apiKeys));
    }
    else
    {
      Assert.Empty(apiKeys);
    }
  }

  [Fact(DisplayName = "SaveAsync: it should save the API key correctly.")]
  public async Task Given_ApiKey_When_SaveAsync_Then_SavedCorrectly()
  {
    TenantId tenantId = TenantId.NewId();

    Role role = new(new UniqueName(new UniqueNameSettings(), "admin"), actorId: null, RoleId.NewId(tenantId));
    await _roleRepository.SaveAsync(role);

    ApiKey apiKey = new(new DisplayName("Test"), _secret, _actorId, ApiKeyId.NewId(tenantId))
    {
      Description = new Description("This is a test API key."),
      ExpiresOn = DateTime.Now.AddDays(1)
    };
    string userId = Guid.NewGuid().ToString();
    apiKey.SetCustomAttribute(new Identifier("UserId"), userId);
    apiKey.Update();
    apiKey.AddRole(role);
    apiKey.Authenticate(SecretString);
    await _apiKeyRepository.SaveAsync(apiKey);

    ApiKeyEntity? entity = await IdentityContext.ApiKeys.AsNoTracking()
      .Include(x => x.Roles)
      .SingleOrDefaultAsync();

    Assert.NotNull(entity);
    Assert.Equal(apiKey.Id.Value, entity.StreamId);
    Assert.Equal(apiKey.Version, entity.Version);
    Assert.Equal(apiKey.CreatedBy?.Value, entity.CreatedBy);
    Assert.Equal(apiKey.CreatedOn.AsUniversalTime(), entity.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(apiKey.UpdatedBy?.Value, entity.UpdatedBy);
    Assert.Equal(apiKey.UpdatedOn.AsUniversalTime(), entity.UpdatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(tenantId.Value, entity.TenantId);
    Assert.Equal(apiKey.EntityId.Value, entity.EntityId);
    Assert.Equal(_secret.Encode(), entity.SecretHash);
    Assert.Equal(apiKey.DisplayName.Value, entity.DisplayName);
    Assert.Equal(apiKey.Description.Value, entity.Description);
    Assert.True(entity.ExpiresOn.HasValue);
    Assert.Equal(apiKey.ExpiresOn.Value.AsUniversalTime(), entity.ExpiresOn.Value, TimeSpan.FromSeconds(1));
    Assert.True(entity.AuthenticatedOn.HasValue);
    Assert.Equal(DateTime.UtcNow, entity.AuthenticatedOn.Value, TimeSpan.FromSeconds(1));
    Assert.Equal($@"{{""UserId"":""{userId}""}}", entity.CustomAttributes);
    Assert.Equal(role.Id.Value, Assert.Single(entity.Roles).StreamId);

    ActorEntity? actor = await IdentityContext.Actors.AsNoTracking().SingleOrDefaultAsync();
    Assert.NotNull(actor);
    Assert.Equal(entity.StreamId, actor.Id);
    Assert.Equal(ActorType.ApiKey, actor.Type);
    Assert.False(actor.IsDeleted);
    Assert.Equal(entity.DisplayName, actor.DisplayName);
    Assert.Null(actor.EmailAddress);
    Assert.Null(actor.PictureUrl);

    CustomAttributeEntity[] customAttributes = await IdentityContext.CustomAttributes.AsNoTracking().ToArrayAsync();
    Assert.Equal(apiKey.CustomAttributes.Count, customAttributes.Length);
    foreach (KeyValuePair<Identifier, string> customAttribute in apiKey.CustomAttributes)
    {
      Assert.Contains(customAttributes, c => c.EntityType == EntityType.ApiKey && c.EntityId == entity.ApiKeyId && c.Key == customAttribute.Key.Value
        && c.Value == customAttribute.Value && c.ValueShortened == customAttribute.Value.Truncate(byte.MaxValue));
    }

    apiKey.Delete();
    await _apiKeyRepository.SaveAsync(apiKey);

    entity = await IdentityContext.ApiKeys.AsNoTracking().SingleOrDefaultAsync();
    Assert.Null(entity);

    actor = await IdentityContext.Actors.AsNoTracking().SingleOrDefaultAsync();
    Assert.NotNull(actor);
    Assert.True(actor.IsDeleted);

    customAttributes = await IdentityContext.CustomAttributes.AsNoTracking().ToArrayAsync();
    Assert.Empty(customAttributes);
  }
}
