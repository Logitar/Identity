using Bogus;
using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.ApiKeys;

[Trait(Traits.Category, Categories.Unit)]
public class ApiKeyAggregateTests
{
  private const string SecretString = "Test123!";

  private readonly Faker _faker = new();
  private readonly UniqueNameSettings _uniqueNameSettings = new();
  private readonly RoleAggregate _role;
  private readonly ApiKeyAggregate _apiKey;

  public ApiKeyAggregateTests()
  {
    _role = new(new UniqueNameUnit(_uniqueNameSettings, "admin"));
    _apiKey = new(new DisplayNameUnit("Default"), new PasswordMock(SecretString));
  }

  [Fact(DisplayName = "AddRole: it should add the role to the API key when it does not have the role.")]
  public void AddRole_it_should_add_the_role_to_the_Api_key_when_it_does_not_have_the_role()
  {
    Assert.False(_apiKey.HasRole(_role));
    Assert.Empty(_apiKey.Roles);

    _apiKey.AddRole(_role);
    Assert.Contains(_apiKey.Changes, changes => changes is ApiKeyRoleAddedEvent @event && @event.RoleId == _role.Id);
    Assert.True(_apiKey.HasRole(_role));
    Assert.Single(_apiKey.Roles, _role.Id);

    _apiKey.ClearChanges();
    _apiKey.AddRole(_role);
    Assert.False(_apiKey.HasChanges);
  }

  [Fact(DisplayName = "AddRole: it should throw TenantMismatchException when the role is in a different tenant.")]
  public void AddRole_it_should_throw_TenantMismatchException_when_the_role_is_in_a_different_tenant()
  {
    TenantId tenantId = new(Guid.NewGuid().ToString());
    RoleAggregate role = new(_role.UniqueName, tenantId);

    var exception = Assert.Throws<TenantMismatchException>(() => _apiKey.AddRole(role));
    Assert.Equal(_apiKey.TenantId, exception.ExpectedTenantId);
    Assert.Equal(role.TenantId, exception.ActualTenantId);
  }

  [Fact(DisplayName = "Authenticate: it should authenticate the API key.")]
  public void Authenticate_it_should_authenticate_the_Api_key()
  {
    _apiKey.Authenticate(SecretString);
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyAuthenticatedEvent @event && @event.ActorId.Value == _apiKey.Id.Value);
  }

  [Fact(DisplayName = "Authenticate: it should authenticate the API key using the specified actor identifier.")]
  public void Authenticate_it_should_authenticate_the_Api_key_using_the_specified_actor_identifier()
  {
    ActorId actorId = ActorId.NewId();
    _apiKey.Authenticate(SecretString, propertyName: null, actorId);
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyAuthenticatedEvent @event && @event.ActorId == actorId);
  }

  [Fact(DisplayName = "Authenticate: it should throw IncorrectApiKeySecretException when the attempted secret is incorrect.")]
  public void Authenticate_it_should_throw_IncorrectApiKeySecretException_when_the_attempted_secret_is_incorrect()
  {
    string secret = SecretString[1..];

    string propertyName = "Secret";
    var exception = Assert.Throws<IncorrectApiKeySecretException>(() => _apiKey.Authenticate(secret, propertyName));
    Assert.Equal(secret, exception.AttemptedSecret);
    Assert.Equal(_apiKey.Id, exception.ApiKeyId);
    Assert.Equal(propertyName, exception.PropertyName);
  }

  [Fact(DisplayName = "Authenticate: it should throw IncorrectApiKeySecretException when the API key has no secret.")]
  public void Authenticate_it_should_throw_IncorrectApiKeySecretException_when_the_Api_key_has_no_secret()
  {
    ApiKeyAggregate apiKey = new(AggregateId.NewId());

    string propertyName = "Secret";
    var exception = Assert.Throws<IncorrectApiKeySecretException>(() => apiKey.Authenticate(SecretString, propertyName));
    Assert.Equal(SecretString, exception.AttemptedSecret);
    Assert.Equal(apiKey.Id, exception.ApiKeyId);
    Assert.Equal(propertyName, exception.PropertyName);
  }

  [Fact(DisplayName = "Authenticate: it should throw ApiKeyHasExpiredException when the API key is expired.")]
  public void Authenticate_it_should_throw_ApiKeyHasExpiredException_when_the_Api_key_is_expired()
  {
    _apiKey.SetExpiration(DateTime.Now.AddMilliseconds(100));

    Thread.Sleep(TimeSpan.FromMilliseconds(100));

    var exception = Assert.Throws<ApiKeyHasExpiredException>(() => _apiKey.Authenticate(SecretString));
    Assert.Equal(_apiKey.Id, exception.ApiKeyId);
  }

  [Fact(DisplayName = "ctor: it should create a new API key with id.")]
  public void ctor_it_should_create_a_new_Api_key_with_id()
  {
    ApiKeyId apiKeyId = new(AggregateId.NewId());

    ApiKeyAggregate apiKey = new(apiKeyId.AggregateId);

    Assert.Equal(apiKeyId, apiKey.Id);
    Assert.Equal(apiKeyId.AggregateId, apiKey.Id.AggregateId);
    Assert.Null(apiKey.TenantId);
  }

  [Fact(DisplayName = "ctor: it should create a new API key with parameters.")]
  public void ctor_it_should_create_a_new_Api_key_with_parameters()
  {
    TenantId tenantId = new(Guid.NewGuid().ToString());
    ActorId actorId = ActorId.NewId();
    ApiKeyId id = new(Guid.NewGuid().ToString());
    PasswordMock secret = new(SecretString);

    ApiKeyAggregate apiKey = new(_apiKey.DisplayName, secret, tenantId, actorId, id);

    Assert.Equal(id, apiKey.Id);
    Assert.Equal(actorId, apiKey.CreatedBy);
    Assert.Equal(tenantId, apiKey.TenantId);
    Assert.Equal(_apiKey.DisplayName, apiKey.DisplayName);
    AssertSecret(apiKey, SecretString);
  }

  [Fact(DisplayName = "Delete: it should delete the API key when it is not deleted.")]
  public void Delete_it_should_delete_the_Api_key_when_it_is_not_deleted()
  {
    Assert.False(_apiKey.IsDeleted);

    _apiKey.Delete();
    Assert.True(_apiKey.IsDeleted);
    Assert.Contains(_apiKey.Changes, change => change is ApiKeyDeletedEvent);

    _apiKey.ClearChanges();
    _apiKey.Delete();
    Assert.False(_apiKey.HasChanges);
  }

  [Fact(DisplayName = "Description: it should change the description when it is different.")]
  public void Description_it_should_change_the_description_when_it_is_different()
  {
    DescriptionUnit description = new("This is the default API key.");
    _apiKey.Description = description;
    Assert.Equal(description, _apiKey.Description);

    _apiKey.Update();

    _apiKey.Description = description;
    AssertHasNoUpdate(_apiKey);
  }

  [Fact(DisplayName = "DisplayName: it should change the display name when it is different.")]
  public void DisplayName_it_should_change_the_display_name_when_it_is_different()
  {
    DisplayNameUnit displayName = new("[LEGACY] Default");
    _apiKey.DisplayName = displayName;
    Assert.Equal(displayName, _apiKey.DisplayName);

    _apiKey.Update();

    _apiKey.DisplayName = displayName;
    AssertHasNoUpdate(_apiKey);
  }

  [Fact(DisplayName = "HasRole: it should return false when the Api key does not have the specified role.")]
  public void HasRole_it_should_return_false_when_the_Api_key_does_not_have_the_specified_role()
  {
    Assert.False(_apiKey.HasRole(_role));
  }

  [Fact(DisplayName = "HasRole: it should return true when the Api key does have the specified role.")]
  public void HasRole_it_should_return_true_when_the_Api_key_does_have_the_specified_role()
  {
    _apiKey.AddRole(_role);
    Assert.True(_apiKey.HasRole(_role));
  }

  [Fact(DisplayName = "IsExpired: it should return false when the Api key has no expiration.")]
  public void IsExpired_it_should_return_false_when_the_Api_key_has_no_expiration()
  {
    Assert.Null(_apiKey.ExpiresOn);
    Assert.False(_apiKey.IsExpired());
  }

  [Fact(DisplayName = "IsExpired: it should return false when the Api key is not expired.")]
  public void IsExpired_it_should_return_false_when_the_Api_key_is_not_expired()
  {
    _apiKey.SetExpiration(DateTime.Now.AddYears(1));

    Assert.False(_apiKey.IsExpired());
  }

  [Fact(DisplayName = "IsExpired: it should return true when the Api key is expired.")]
  public void IsExpired_it_should_return_true_when_the_Api_key_is_expired()
  {
    _apiKey.SetExpiration(DateTime.Now.AddMilliseconds(100));

    Thread.Sleep(TimeSpan.FromMilliseconds(100));

    Assert.True(_apiKey.IsExpired());
  }

  [Fact(DisplayName = "RemoveCustomAttribute: it should remove an existing custom attribute.")]
  public void RemoveCustomAttribute_it_should_remove_an_existing_custom_attribute()
  {
    string key = "remove_users";

    _apiKey.SetCustomAttribute(key, bool.TrueString);
    _apiKey.Update();

    _apiKey.RemoveCustomAttribute($"  {key}  ");
    _apiKey.Update();
    Assert.False(_apiKey.CustomAttributes.ContainsKey(key));

    _apiKey.RemoveCustomAttribute(key);
    AssertHasNoUpdate(_apiKey);
  }

  [Fact(DisplayName = "RemoveRole: it should remove the role from the API key when it has the role.")]
  public void RemoveRole_it_should_remove_the_role_from_the_Api_key_when_it_has_the_role()
  {
    _apiKey.AddRole(_role);
    Assert.True(_apiKey.HasRole(_role));
    Assert.Single(_apiKey.Roles, _role.Id);
    _apiKey.ClearChanges();

    _apiKey.RemoveRole(_role);
    Assert.Contains(_apiKey.Changes, changes => changes is ApiKeyRoleRemovedEvent @event && @event.RoleId == _role.Id);
    Assert.False(_apiKey.HasRole(_role));
    Assert.Empty(_apiKey.Roles);

    _apiKey.ClearChanges();
    _apiKey.RemoveRole(_role);
    Assert.False(_apiKey.HasChanges);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should set a custom attribute when it is different.")]
  public void SetCustomAttribute_it_should_set_a_custom_attribute_when_it_is_different()
  {
    string key = "  remove_users  ";
    string value = $"  {bool.TrueString}  ";
    _apiKey.SetCustomAttribute(key, value);
    Assert.Equal(value.Trim(), _apiKey.CustomAttributes[key.Trim()]);

    _apiKey.Update();

    _apiKey.SetCustomAttribute(key, value);
    AssertHasNoUpdate(_apiKey);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should throw ValidationException when the key or value is not valid.")]
  public void SetCustomAttribute_it_should_throw_ValidationException_when_the_key_or_value_is_not_valid()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _apiKey.SetCustomAttribute("   ", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    string key = _faker.Random.String(CustomAttributeKeyValidator.MaximumLength + 1, minChar: 'A', maxChar: 'Z');
    exception = Assert.Throws<FluentValidation.ValidationException>(() => _apiKey.SetCustomAttribute(key, "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _apiKey.SetCustomAttribute("-key", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _apiKey.SetCustomAttribute("key", "     "));
    Assert.All(exception.Errors, e => Assert.Equal("Value", e.PropertyName));
  }

  [Fact(DisplayName = "SetExpiration: it should change the expiration when it is different.")]
  public void SetExpiration_it_should_change_the_expiration_when_it_is_different()
  {
    DateTime expiresOn = DateTime.Now.AddYears(1);

    _apiKey.SetExpiration(expiresOn);
    Assert.Equal(expiresOn, _apiKey.ExpiresOn);

    _apiKey.Update();

    _apiKey.SetExpiration(expiresOn);
    AssertHasNoUpdate(_apiKey);
  }

  [Fact(DisplayName = "SetExpiration: it should throw ValidationException when the expiration is not in the future.")]
  public void SetExpiration_it_should_throw_ValidationException_when_the_expiration_is_not_in_the_future()
  {
    DateTime expiresOn = DateTime.Now.AddDays(-1);
    string propertyName = "ExpiresOn";

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _apiKey.SetExpiration(expiresOn, propertyName));
    ValidationFailure failure = Assert.Single(exception.Errors);
    Assert.Equal(expiresOn, failure.AttemptedValue);
    Assert.Equal("FutureValidator", failure.ErrorCode);
    Assert.Equal(propertyName, failure.PropertyName);
  }

  [Fact(DisplayName = "SetExpiration: it should throw ValidationException when the expiration is postponed.")]
  public void SetExpiration_it_should_throw_ValidationException_when_the_expiration_is_postponed()
  {
    _apiKey.SetExpiration(DateTime.Now.AddMonths(6));

    DateTime expiresOn = DateTime.Now.AddYears(1);
    string propertyName = "ExpiresOn";

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _apiKey.SetExpiration(expiresOn, propertyName));
    ValidationFailure failure = Assert.Single(exception.Errors);
    Assert.Equal(expiresOn, failure.AttemptedValue);
    Assert.Equal("LessThanOrEqualValidator", failure.ErrorCode);
    Assert.Equal(propertyName, failure.PropertyName);
  }

  [Fact(DisplayName = "ToString: it should return the correct string representation.")]
  public void ToString_it_should_return_the_correct_string_representation()
  {
    Assert.StartsWith($"{_apiKey.DisplayName.Value} | ", _apiKey.ToString());
  }

  [Fact(DisplayName = "Update: it should update the API key when it has changes.")]
  public void Update_it_should_update_the_Api_key_when_it_has_changes()
  {
    ActorId actorId = ActorId.NewId();

    _apiKey.Description = new DescriptionUnit("This is the default API key.");
    _apiKey.Update(actorId);
    Assert.Equal(actorId, _apiKey.UpdatedBy);

    long version = _apiKey.Version;
    _apiKey.Update(actorId);
    Assert.Equal(version, _apiKey.Version);
  }

  private static void AssertHasNoUpdate(ApiKeyAggregate apiKey)
  {
    FieldInfo? field = apiKey.GetType().GetField("_updated", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    ApiKeyUpdatedEvent? updated = field.GetValue(apiKey) as ApiKeyUpdatedEvent;
    Assert.NotNull(updated);
    Assert.False(updated.HasChanges);
  }
  private static void AssertSecret(ApiKeyAggregate apiKey, string? secret)
  {
    FieldInfo? field = apiKey.GetType().GetField("_secret", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    Password? instance = field.GetValue(apiKey) as Password;
    if (secret == null)
    {
      Assert.Null(instance);
    }
    else
    {
      Assert.NotNull(instance);
      Assert.True(instance.IsMatch(secret));
    }
  }
}
