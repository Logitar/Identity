using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.Infrastructure.Passwords.Pbkdf2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

[Trait(Traits.Category, Categories.Integration)]
public class OneTimePasswordSqlServerIntegrationTests : IntegrationTests
{
  private const string PasswordString = "671359";

  private readonly IOneTimePasswordRepository _oneTimePasswordRepository;

  private readonly ActorId _actorId = ActorId.NewId();
  private readonly Password _password;

  public OneTimePasswordSqlServerIntegrationTests() : base(DatabaseProvider.SqlServer)
  {
    _oneTimePasswordRepository = ServiceProvider.GetRequiredService<IOneTimePasswordRepository>();

    Pbkdf2Settings pbkdf2 = new();
    _password = new Pbkdf2Password(PasswordString, pbkdf2.Algorithm, pbkdf2.Iterations, pbkdf2.SaltLength, pbkdf2.HashLength);
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a deletion status.")]
  [InlineData(null)]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_Deleted_When_LoadAsync_Then_CorrectResult(bool? isDeleted)
  {
    OneTimePassword oneTimePassword1 = new(_password);
    OneTimePassword oneTimePassword2 = new(_password);
    oneTimePassword2.Delete();
    OneTimePassword oneTimePassword3 = new(_password);
    if (isDeleted == true)
    {
      oneTimePassword3.Delete();
    }
    if (isDeleted.HasValue)
    {
      await _oneTimePasswordRepository.SaveAsync([oneTimePassword1, oneTimePassword2, oneTimePassword3]);
    }

    IReadOnlyCollection<OneTimePassword> oneTimePasswords = await _oneTimePasswordRepository.LoadAsync(isDeleted);

    if (isDeleted.HasValue)
    {
      Assert.Equal(2, oneTimePasswords.Count);
      Assert.Contains(oneTimePassword3, oneTimePasswords);
      Assert.Contains(isDeleted.Value ? oneTimePassword2 : oneTimePassword1, oneTimePasswords);
    }
    else
    {
      Assert.Empty(oneTimePasswords);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given IDs and a deletion status.")]
  [InlineData(null)]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_IdsDeleted_When_LoadAsync_Then_CorrectResult(bool? isDeleted)
  {
    OneTimePassword oneTimePassword1 = new(_password);
    OneTimePassword oneTimePassword2 = new(_password);
    oneTimePassword2.Delete();
    OneTimePassword oneTimePassword3 = new(_password);
    if (isDeleted == true)
    {
      oneTimePassword3.Delete();
    }
    OneTimePassword oneTimePassword4 = new(_password);
    if (isDeleted == true)
    {
      oneTimePassword4.Delete();
    }
    if (isDeleted.HasValue)
    {
      await _oneTimePasswordRepository.SaveAsync([oneTimePassword1, oneTimePassword2, oneTimePassword3, oneTimePassword4]);
    }

    IReadOnlyCollection<OneTimePassword> oneTimePasswords = await _oneTimePasswordRepository.LoadAsync([oneTimePassword1.Id, oneTimePassword2.Id, oneTimePassword3.Id, OneTimePasswordId.NewId()], isDeleted);

    if (isDeleted.HasValue)
    {
      Assert.Equal(2, oneTimePasswords.Count);
      Assert.Contains(oneTimePassword3, oneTimePasswords);
      Assert.Contains(isDeleted.Value ? oneTimePassword2 : oneTimePassword1, oneTimePasswords);
    }
    else
    {
      Assert.Empty(oneTimePasswords);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given an ID, a version and a deletion status.")]
  [InlineData(false, null, false)]
  [InlineData(true, null, false)]
  [InlineData(true, false, false)]
  [InlineData(true, true, true)]
  public async Task Given_IdVersionDeleted_When_LoadAsync_Then_CorrectResult(bool found, bool? isDeleted, bool withVersion)
  {
    OneTimePassword oneTimePassword = new(_password);
    if (isDeleted == true)
    {
      oneTimePassword.Delete();
    }
    if (found)
    {
      await _oneTimePasswordRepository.SaveAsync(oneTimePassword);
    }

    long? version = withVersion ? 1 : null;
    OneTimePassword? result = await _oneTimePasswordRepository.LoadAsync(oneTimePassword.Id, version, isDeleted);
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
        Assert.Equal(oneTimePassword.Version, result.Version);
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
    OneTimePassword oneTimePassword1 = new(_password);
    TenantId tenantId = tenantIdValue == null ? TenantId.NewId() : new(tenantIdValue);
    OneTimePassword oneTimePassword2 = new(_password, expiresOn: null, maximumAttempts: null, actorId: null, OneTimePasswordId.NewId(tenantId));
    if (found)
    {
      await _oneTimePasswordRepository.SaveAsync([oneTimePassword1, oneTimePassword2]);
    }

    IReadOnlyCollection<OneTimePassword> oneTimePasswords = await _oneTimePasswordRepository.LoadAsync(tenantIdValue == null ? null : tenantId);

    if (found)
    {
      Assert.Equal(tenantIdValue == null ? oneTimePassword1 : oneTimePassword2, Assert.Single(oneTimePasswords));
    }
    else
    {
      Assert.Empty(oneTimePasswords);
    }
  }

  [Fact(DisplayName = "SaveAsync: it should save the oneTimePassword correctly.")]
  public async Task Given_OneTimePassword_When_SaveAsync_Then_SavedCorrectly()
  {
    TenantId tenantId = TenantId.NewId();

    DateTime expiresOn = DateTime.Now.AddHours(1);
    int maximumAttempts = 5;
    OneTimePassword oneTimePassword = new(_password, expiresOn, maximumAttempts, _actorId, OneTimePasswordId.NewId(tenantId));
    Assert.Throws<IncorrectOneTimePasswordPasswordException>(() => oneTimePassword.Validate("invalid"));
    oneTimePassword.Validate(PasswordString, _actorId);
    string userId = Guid.NewGuid().ToString();
    oneTimePassword.SetCustomAttribute(new Identifier("UserId"), userId);
    oneTimePassword.Update();
    await _oneTimePasswordRepository.SaveAsync(oneTimePassword);

    OneTimePasswordEntity? entity = await IdentityContext.OneTimePasswords.AsNoTracking().SingleOrDefaultAsync();

    Assert.NotNull(entity);
    Assert.Equal(oneTimePassword.Id.Value, entity.StreamId);
    Assert.Equal(oneTimePassword.Version, entity.Version);
    Assert.Equal(oneTimePassword.CreatedBy?.Value, entity.CreatedBy);
    Assert.Equal(oneTimePassword.CreatedOn.AsUniversalTime(), entity.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(oneTimePassword.UpdatedBy?.Value, entity.UpdatedBy);
    Assert.Equal(oneTimePassword.UpdatedOn.AsUniversalTime(), entity.UpdatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(tenantId.Value, entity.TenantId);
    Assert.Equal(oneTimePassword.EntityId.Value, entity.EntityId);
    Assert.Equal(_password.Encode(), entity.PasswordHash);
    Assert.True(entity.ExpiresOn.HasValue);
    Assert.Equal(expiresOn.AsUniversalTime(), entity.ExpiresOn.Value, TimeSpan.FromSeconds(1));
    Assert.Equal(maximumAttempts, entity.MaximumAttempts);
    Assert.Equal(2, entity.AttemptCount);
    Assert.True(entity.HasValidationSucceeded);
    Assert.Equal($@"{{""UserId"":""{userId}""}}", entity.CustomAttributes);

    CustomAttributeEntity[] customAttributes = await IdentityContext.CustomAttributes.AsNoTracking().ToArrayAsync();
    Assert.Equal(oneTimePassword.CustomAttributes.Count, customAttributes.Length);
    foreach (KeyValuePair<Identifier, string> customAttribute in oneTimePassword.CustomAttributes)
    {
      Assert.Contains(customAttributes, c => c.EntityType == EntityType.OneTimePassword && c.EntityId == entity.OneTimePasswordId && c.Key == customAttribute.Key.Value
        && c.Value == customAttribute.Value && c.ValueShortened == customAttribute.Value.Truncate(byte.MaxValue));
    }

    oneTimePassword.Delete();
    await _oneTimePasswordRepository.SaveAsync(oneTimePassword);

    entity = await IdentityContext.OneTimePasswords.AsNoTracking().SingleOrDefaultAsync();
    Assert.Null(entity);

    customAttributes = await IdentityContext.CustomAttributes.AsNoTracking().ToArrayAsync();
    Assert.Empty(customAttributes);
  }
}
