﻿using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Events;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core.ApiKeys;

[Trait(Traits.Category, Categories.Unit)]
public class ApiKeyTests
{
  private const string SecretString = "P@s$W0rD";

  private readonly Base64Password _secret = new(SecretString);
  private readonly ApiKey _apiKey;

  public ApiKeyTests()
  {
    _apiKey = new(new DisplayName("Test API Key"), _secret);
  }

  [Fact(DisplayName = "AddRole: it should add a role.")]
  public void Given_Role_When_AddRole_Then_RoleAdded()
  {
    Role role = new(new UniqueName(new UniqueNameSettings(), "manage_api"));

    _apiKey.AddRole(role);
    Assert.Contains(role.Id, _apiKey.Roles);
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyRoleAdded added && added.RoleId == role.Id);

    _apiKey.ClearChanges();
    _apiKey.AddRole(role);
    Assert.False(_apiKey.HasChanges);
    Assert.Empty(_apiKey.Changes);
  }

  [Fact(DisplayName = "AddRole: it should throw TenantMismatchException when the role is in another tenant.")]
  public void Given_DifferentTenants_When_AddRole_Then_TenantMismatchException()
  {
    Role role = new(new UniqueName(new UniqueNameSettings(), "manage_api"), actorId: null, new RoleId(TenantId.NewId(), EntityId.NewId()));

    var exception = Assert.Throws<TenantMismatchException>(() => _apiKey.AddRole(role));
    Assert.Equal(_apiKey.TenantId?.Value, exception.ExpectedTenantId);
    Assert.Equal(role.TenantId?.Value, exception.ActualTenantId);
  }

  [Theory(DisplayName = "Authenticate: it should authenticate the API key.")]
  [InlineData(null)]
  [InlineData("SYSTEM")]
  public void Given_CorrectSecret_When_Authenticate_Then_Authenticated(string? actorIdValue)
  {
    ActorId? actorId = actorIdValue == null ? null : new(actorIdValue);

    _apiKey.Authenticate(SecretString, actorId);

    ApiKeyAuthenticated? authenticated = _apiKey.Changes.Single(change => change is ApiKeyAuthenticated) as ApiKeyAuthenticated;
    Assert.NotNull(authenticated);
    Assert.Equal(authenticated.OccurredOn, _apiKey.AuthenticatedOn);
    Assert.Equal(actorId ?? new ActorId(_apiKey.Id.Value), authenticated.ActorId);
  }

  [Fact(DisplayName = "Authenticate: it should throw ApiKeyIsExpiredException when the API key is expired.")]
  public void Given_Expired_When_Authenticate_Then_ApiKeyIsExpiredException()
  {
    _apiKey.ExpiresOn = DateTime.Now.AddMilliseconds(50);
    Thread.Sleep(50);

    var exception = Assert.Throws<ApiKeyIsExpiredException>(() => _apiKey.Authenticate(SecretString));
    Assert.Equal(_apiKey.Id.Value, exception.ApiKeyId);
  }

  [Fact(DisplayName = "Authenticate: it should throw IncorrectApiKeySecretException when the secret is not correct.")]
  public void Given_IncorrectSecret_When_Authenticate_Then_IncorrectApiKeySecretException()
  {
    string secret = "Test123!";
    var exception = Assert.Throws<IncorrectApiKeySecretException>(() => _apiKey.Authenticate(secret));
    Assert.Equal(_apiKey.Id.Value, exception.ApiKeyId);
    Assert.Equal(secret, exception.AttemptedSecret);
  }

  [Theory(DisplayName = "ctor: it should construct the correct API key.")]
  [InlineData(null, true)]
  [InlineData("SYSTEM", false)]
  public void Given_Parameters_When_ctor_Then_CorrectApiKeyConstructed(string? actorIdValue, bool generateId)
  {
    ActorId? actorId = actorIdValue == null ? null : new(actorIdValue);
    ApiKeyId? id = generateId ? ApiKeyId.NewId() : null;

    ApiKey apiKey = new(_apiKey.DisplayName, _secret, actorId, id);

    if (id.HasValue)
    {
      Assert.Equal(id.Value, apiKey.Id);
    }
    else
    {
      Assert.Null(apiKey.TenantId);
      Assert.NotEqual(Guid.Empty, apiKey.EntityId.ToGuid());
    }

    Assert.Equal(actorId, apiKey.CreatedBy);
    Assert.Equal(_apiKey.DisplayName, apiKey.DisplayName);

    Assert.Equal(_secret, apiKey.GetType().GetField("_secret", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(apiKey));
  }

  [Fact(DisplayName = "Delete: it should delete the API key.")]
  public void Given_ApiKey_When_Delete_Then_Deleted()
  {
    _apiKey.Delete();
    Assert.True(_apiKey.IsDeleted);

    _apiKey.ClearChanges();
    _apiKey.Delete();
    Assert.False(_apiKey.HasChanges);
    Assert.Empty(_apiKey.Changes);
  }

  [Fact(DisplayName = "Description: it should handle the updates correctly.")]
  public void Given_DescriptionUpdates_When_setDescription_Then_UpdatesHandledCorrectly()
  {
    _apiKey.ClearChanges();

    _apiKey.Description = null;
    _apiKey.Update();
    Assert.False(_apiKey.HasChanges);
    Assert.Empty(_apiKey.Changes);

    _apiKey.Description = new Description("This is a new API key.");
    _apiKey.Update();
    Assert.True(_apiKey.HasChanges);
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyUpdated updated && updated.Description?.Value == _apiKey.Description);
  }

  [Fact(DisplayName = "DisplayName: it should handle the updates correctly.")]
  public void Given_DisplayNameUpdates_When_setDisplayName_Then_UpdatesHandledCorrectly()
  {
    _apiKey.ClearChanges();

    _apiKey.DisplayName = _apiKey.DisplayName;
    _apiKey.Update();
    Assert.False(_apiKey.HasChanges);
    Assert.Empty(_apiKey.Changes);

    _apiKey.DisplayName = new DisplayName("New API Key");
    _apiKey.Update();
    Assert.True(_apiKey.HasChanges);
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyUpdated updated && updated.DisplayName == _apiKey.DisplayName);
  }

  [Fact(DisplayName = "ExpiresOn: it should handle the updates correctly.")]
  public void Given_ExpiresOnUpdates_When_setExpiresOn_Then_UpdatesHandledCorrectly()
  {
    _apiKey.ClearChanges();

    _apiKey.ExpiresOn = _apiKey.ExpiresOn;
    _apiKey.Update();
    Assert.False(_apiKey.HasChanges);
    Assert.Empty(_apiKey.Changes);

    _apiKey.ExpiresOn = DateTime.Now.AddYears(1);
    _apiKey.Update();
    Assert.True(_apiKey.HasChanges);
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyUpdated updated && updated.ExpiresOn == _apiKey.ExpiresOn);
  }

  [Fact(DisplayName = "ExpiresOn: it should throw ArgumentException when the date and time are more in the future than the current expiration.")]
  public void Given_ExpirationExtended_When_setExpiresOn_Then_ArgumentException()
  {
    _apiKey.ExpiresOn = DateTime.Now.AddDays(90);

    var exception = Assert.Throws<ArgumentException>(() => _apiKey.ExpiresOn = DateTime.Now.AddYears(1));
    Assert.Equal("ExpiresOn", exception.ParamName);
    Assert.StartsWith("The API key expiration cannot be extended.", exception.Message);
  }

  [Fact(DisplayName = "ExpiresOn: it should throw ArgumentOutOfRangeException when the date and time are not set in the future.")]
  public void Given_PastExpiration_When_setExpiresOn_Then_ArgumentOutOfRangeException()
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _apiKey.ExpiresOn = DateTime.Now);
    Assert.Equal("ExpiresOn", exception.ParamName);
    Assert.StartsWith("The expiration date and time must be set in the future.", exception.Message);
  }

  [Fact(DisplayName = "IsExpired: it should return false when the API is not expired.")]
  public void Given_NotExpired_When_IsExpired_Then_FalseReturned()
  {
    Assert.False(_apiKey.IsExpired());

    _apiKey.ExpiresOn = DateTime.Now.AddDays(90);
    Assert.False(_apiKey.IsExpired());
  }

  [Fact(DisplayName = "IsExpired: it should return true when the API is expired.")]
  public void Given_Expired_When_IsExpired_Then_TrueReturned()
  {
    _apiKey.ExpiresOn = DateTime.Now.AddDays(90);
    Assert.True(_apiKey.IsExpired(DateTime.Now.AddYears(1)));
  }

  [Fact(DisplayName = "It should have the correct IDs.")]
  public void Given_ApiKey_When_getIds_Then_CorrectIds()
  {
    ApiKeyId id = new(TenantId.NewId(), EntityId.NewId());
    ApiKey apiKey = new(_apiKey.DisplayName, _secret, actorId: null, id);
    Assert.Equal(id, apiKey.Id);
    Assert.Equal(id.TenantId, apiKey.TenantId);
    Assert.Equal(id.EntityId, apiKey.EntityId);
  }

  [Fact(DisplayName = "RemoveCustomAttribute: it should remove the custom attribute.")]
  public void Given_CustomAttributes_When_RemoveCustomAttribute_Then_CustomAttributeRemoved()
  {
    Identifier key = new("UserId");
    _apiKey.SetCustomAttribute(key, UserId.NewId().Value);
    _apiKey.Update();

    _apiKey.RemoveCustomAttribute(key);
    _apiKey.Update();
    Assert.False(_apiKey.CustomAttributes.ContainsKey(key));
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyUpdated updated && updated.CustomAttributes[key] == null);

    _apiKey.ClearChanges();
    _apiKey.RemoveCustomAttribute(key);
    Assert.False(_apiKey.HasChanges);
    Assert.Empty(_apiKey.Changes);
  }

  [Fact(DisplayName = "RemoveRole: it should remove a role.")]
  public void Given_Role_When_RemoveRole_Then_RoleRemoved()
  {
    Role role = new(new UniqueName(new UniqueNameSettings(), "manage_api"));
    _apiKey.AddRole(role);
    Assert.True(_apiKey.HasRole(role));

    _apiKey.RemoveRole(role);
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyRoleRemoved removed && removed.RoleId == role.Id);

    _apiKey.ClearChanges();
    _apiKey.RemoveRole(role);
    Assert.False(_apiKey.HasChanges);
    Assert.Empty(_apiKey.Changes);
    Assert.False(_apiKey.HasRole(role));
  }

  [Theory(DisplayName = "SetCustomAttribute: it should remove the custom attribute when the value is null, empty or white-space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Given_EmptyValue_When_SetCustomAttribute_Then_CustomAttributeRemoved(string? value)
  {
    Identifier key = new("UserId");
    _apiKey.SetCustomAttribute(key, UserId.NewId().Value);
    _apiKey.Update();

    _apiKey.SetCustomAttribute(key, value!);
    _apiKey.Update();
    Assert.False(_apiKey.CustomAttributes.ContainsKey(key));
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyUpdated updated && updated.CustomAttributes[key] == null);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should set a custom attribute.")]
  public void Given_CustomAttribute_When_SetCustomAttribute_Then_CustomAttributeSet()
  {
    Identifier key = new("UserId");
    string value = $"  {UserId.NewId().Value}  ";

    _apiKey.SetCustomAttribute(key, value);
    _apiKey.Update();
    Assert.Equal(_apiKey.CustomAttributes[key], value.Trim());
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyUpdated updated && updated.CustomAttributes[key] == value.Trim());

    _apiKey.ClearChanges();
    _apiKey.SetCustomAttribute(key, value.Trim());
    _apiKey.Update();
    Assert.False(_apiKey.HasChanges);
    Assert.Empty(_apiKey.Changes);
  }

  [Fact(DisplayName = "ToString: it should return the correct string representation.")]
  public void Given_ApiKey_When_ToString_Then_CorrectString()
  {
    Assert.StartsWith(_apiKey.DisplayName.Value, _apiKey.ToString());
  }

  [Theory(DisplayName = "Update: it should update the API key.")]
  [InlineData(null)]
  [InlineData("SYSTEM")]
  public void Given_Updates_When_Update_Then_ApiKeyUpdated(string? actorIdValue)
  {
    ActorId? actorId = actorIdValue == null ? null : new(actorIdValue);

    _apiKey.ClearChanges();
    _apiKey.Update();
    Assert.False(_apiKey.HasChanges);
    Assert.Empty(_apiKey.Changes);

    _apiKey.SetCustomAttribute(new Identifier("UserId"), UserId.NewId().Value);
    _apiKey.Update(actorId);
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyUpdated updated && updated.ActorId == actorId && (updated.OccurredOn - DateTime.Now) < TimeSpan.FromSeconds(1));
  }
}
