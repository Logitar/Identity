using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.Infrastructure.Passwords.Pbkdf2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

[Trait(Traits.Category, Categories.Integration)]
public class RolePostgresIntegrationTests : IntegrationTests
{
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IRoleRepository _roleRepository;
  private readonly IUserRepository _userRepository;

  private readonly ActorId _actorId = ActorId.NewId();
  private readonly Password _secret;
  private readonly UniqueNameSettings _uniqueNameSettings = new();

  public RolePostgresIntegrationTests() : base(DatabaseProvider.PostgreSQL)
  {
    _apiKeyRepository = ServiceProvider.GetRequiredService<IApiKeyRepository>();
    _roleRepository = ServiceProvider.GetRequiredService<IRoleRepository>();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();

    Pbkdf2Settings pbkdf2 = new();
    _secret = new Pbkdf2Password("P@s$W0rD", pbkdf2.Algorithm, pbkdf2.Iterations, pbkdf2.SaltLength, pbkdf2.HashLength);
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a deletion status.")]
  [InlineData(null)]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_Deleted_When_LoadAsync_Then_CorrectResult(bool? isDeleted)
  {
    Role role1 = new(new UniqueName(_uniqueNameSettings, "admin"));
    Role role2 = new(new UniqueName(_uniqueNameSettings, "guest"));
    role2.Delete();
    Role role3 = new(new UniqueName(_uniqueNameSettings, "superuser"));
    if (isDeleted == true)
    {
      role3.Delete();
    }
    if (isDeleted.HasValue)
    {
      await _roleRepository.SaveAsync([role1, role2, role3]);
    }

    IReadOnlyCollection<Role> roles = await _roleRepository.LoadAsync(isDeleted);

    if (isDeleted.HasValue)
    {
      Assert.Equal(2, roles.Count);
      Assert.Contains(role3, roles);
      Assert.Contains(isDeleted.Value ? role2 : role1, roles);
    }
    else
    {
      Assert.Empty(roles);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given IDs and a deletion status.")]
  [InlineData(null)]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_IdsDeleted_When_LoadAsync_Then_CorrectResult(bool? isDeleted)
  {
    Role role1 = new(new UniqueName(_uniqueNameSettings, "admin"));
    Role role2 = new(new UniqueName(_uniqueNameSettings, "guest"));
    role2.Delete();
    Role role3 = new(new UniqueName(_uniqueNameSettings, "superuser"));
    if (isDeleted == true)
    {
      role3.Delete();
    }
    Role role4 = new(new UniqueName(_uniqueNameSettings, "player"));
    if (isDeleted == true)
    {
      role4.Delete();
    }
    if (isDeleted.HasValue)
    {
      await _roleRepository.SaveAsync([role1, role2, role3, role4]);
    }

    IReadOnlyCollection<Role> roles = await _roleRepository.LoadAsync([role1.Id, role2.Id, role3.Id, RoleId.NewId()], isDeleted);

    if (isDeleted.HasValue)
    {
      Assert.Equal(2, roles.Count);
      Assert.Contains(role3, roles);
      Assert.Contains(isDeleted.Value ? role2 : role1, roles);
    }
    else
    {
      Assert.Empty(roles);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given an ID, a version and a deletion status.")]
  [InlineData(false, null, false)]
  [InlineData(true, null, false)]
  [InlineData(true, false, false)]
  [InlineData(true, true, true)]
  public async Task Given_IdVersionDeleted_When_LoadAsync_Then_CorrectResult(bool found, bool? isDeleted, bool withVersion)
  {
    Role role = new(new UniqueName(_uniqueNameSettings, "admin"));
    if (isDeleted == true)
    {
      role.Delete();
    }
    if (found)
    {
      await _roleRepository.SaveAsync(role);
    }

    long? version = withVersion ? 1 : null;
    Role? result = await _roleRepository.LoadAsync(role.Id, version, isDeleted);
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
        Assert.Equal(role.Version, result.Version);
        Assert.Equal(isDeleted ?? false, result.IsDeleted);
      }
    }
    else
    {
      Assert.Null(result);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a tenant ID.")]
  [InlineData(false, null)]
  [InlineData(true, null)]
  [InlineData(true, "9699e381-3965-4d77-ada2-e0b3742c3b91")]
  public async Task Given_TenantId_When_LoadAsync_Then_CorrectResults(bool found, string? tenantIdValue)
  {
    Role role1 = new(new UniqueName(_uniqueNameSettings, "admin"));
    TenantId tenantId = tenantIdValue == null ? TenantId.NewId() : new(tenantIdValue);
    Role role2 = new(new UniqueName(_uniqueNameSettings, "guest"), actorId: null, RoleId.NewId(tenantId));
    if (found)
    {
      await _roleRepository.SaveAsync([role1, role2]);
    }

    IReadOnlyCollection<Role> roles = await _roleRepository.LoadAsync(tenantIdValue == null ? null : tenantId);

    if (found)
    {
      Assert.Equal(tenantIdValue == null ? role1 : role2, Assert.Single(roles));
    }
    else
    {
      Assert.Empty(roles);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a tenant ID and a unique name.")]
  [InlineData(null, null)]
  [InlineData(null, "admin")]
  [InlineData("7ecb850f-db7e-4400-b066-563142a005d4", "guest")]
  public async Task Given_TenantIdUniqueName_When_LoadAsync_Then_CorrectResult(string? tenantIdValue, string? uniqueNameValue)
  {
    TenantId? tenantId = tenantIdValue == null ? null : new(tenantIdValue);
    UniqueName uniqueName = new(_uniqueNameSettings, uniqueNameValue ?? "default");
    Role role = new(uniqueName, actorId: null, RoleId.NewId(tenantId));
    await _roleRepository.SaveAsync(role);

    Role? result = await _roleRepository.LoadAsync(tenantId, uniqueNameValue == null ? new UniqueName(_uniqueNameSettings, "null") : uniqueName);

    if (uniqueNameValue == null)
    {
      Assert.Null(result);
    }
    else
    {
      Assert.NotNull(result);
      Assert.Equal(role, result);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given an API key.")]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_ApiKey_When_LoadAsync_Then_CorrectResults(bool found)
  {
    Role role = new(new UniqueName(_uniqueNameSettings, "admin"));
    await _roleRepository.SaveAsync(role);

    ApiKey apiKey = new(new DisplayName("Test"), _secret);
    if (found)
    {
      apiKey.AddRole(role);
    }
    await _apiKeyRepository.SaveAsync(apiKey);

    IReadOnlyCollection<Role> roles = await _roleRepository.LoadAsync(apiKey);

    if (found)
    {
      Assert.Equal(role, Assert.Single(roles));
    }
    else
    {
      Assert.Empty(roles);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a user.")]
  [InlineData(false)]
  [InlineData(true)]
  public async Task GivenUser_When_LoadAsync_Then_CorrectResults(bool found)
  {
    Role role = new(new UniqueName(_uniqueNameSettings, "admin"));
    await _roleRepository.SaveAsync(role);

    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    if (found)
    {
      user.AddRole(role);
    }
    await _userRepository.SaveAsync(user);

    IReadOnlyCollection<Role> roles = await _roleRepository.LoadAsync(user);

    if (found)
    {
      Assert.Equal(role, Assert.Single(roles));
    }
    else
    {
      Assert.Empty(roles);
    }
  }

  [Fact(DisplayName = "SaveAsync: it should save the role correctly.")]
  public async Task Given_Role_When_SaveAsync_Then_SavedCorrectly()
  {
    TenantId tenantId = TenantId.NewId();

    Role role = new(new UniqueName(_uniqueNameSettings, "admin"), _actorId, RoleId.NewId(tenantId))
    {
      DisplayName = new DisplayName("Administrator"),
      Description = new Description("This is the administration role.")
    };
    role.SetCustomAttribute(new Identifier("manage_api"), bool.TrueString);
    role.Update();
    role.SetUniqueName(new UniqueName(_uniqueNameSettings, "administrator"), _actorId);
    await _roleRepository.SaveAsync(role);

    RoleEntity? entity = await IdentityContext.Roles.AsNoTracking().SingleOrDefaultAsync();

    Assert.NotNull(entity);
    Assert.Equal(role.Id.Value, entity.StreamId);
    Assert.Equal(role.Version, entity.Version);
    Assert.Equal(role.CreatedBy?.Value, entity.CreatedBy);
    Assert.Equal(role.CreatedOn.AsUniversalTime(), entity.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(role.UpdatedBy?.Value, entity.UpdatedBy);
    Assert.Equal(role.UpdatedOn.AsUniversalTime(), entity.UpdatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(tenantId.Value, entity.TenantId);
    Assert.Equal(role.EntityId.Value, entity.EntityId);
    Assert.Equal(role.UniqueName.Value, entity.UniqueName);
    Assert.Equal(role.UniqueName.Value.ToUpperInvariant(), entity.UniqueNameNormalized);
    Assert.Equal(role.DisplayName.Value, entity.DisplayName);
    Assert.Equal(role.Description.Value, entity.Description);
    Assert.Equal(@"{""manage_api"":""True""}", entity.CustomAttributes);

    CustomAttributeEntity[] customAttributes = await IdentityContext.CustomAttributes.AsNoTracking().ToArrayAsync();
    Assert.Equal(role.CustomAttributes.Count, customAttributes.Length);
    foreach (KeyValuePair<Identifier, string> customAttribute in role.CustomAttributes)
    {
      Assert.Contains(customAttributes, c => c.EntityType == EntityType.Role && c.EntityId == entity.RoleId && c.Key == customAttribute.Key.Value
        && c.Value == customAttribute.Value && c.ValueShortened == customAttribute.Value.Truncate(byte.MaxValue));
    }

    role.Delete();
    await _roleRepository.SaveAsync(role);

    entity = await IdentityContext.Roles.AsNoTracking().SingleOrDefaultAsync();
    Assert.Null(entity);

    customAttributes = await IdentityContext.CustomAttributes.AsNoTracking().ToArrayAsync();
    Assert.Empty(customAttributes);
  }
}
