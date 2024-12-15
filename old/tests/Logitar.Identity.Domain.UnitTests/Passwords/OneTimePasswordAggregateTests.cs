using Bogus;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords.Events;
using Logitar.Identity.Domain.Passwords.Validators;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Passwords;

[Trait(Traits.Category, Categories.Unit)]
public class OneTimePasswordAggregateTests
{
  private const string PasswordString = "420742";

  private readonly Faker _faker = new();

  private readonly Base64Password _password = new(PasswordString);
  private readonly OneTimePasswordAggregate _oneTimePassword;

  public OneTimePasswordAggregateTests()
  {
    _oneTimePassword = new(_password);
  }

  [Fact(DisplayName = "ctor: it should create a new One-Time Password with parameters.")]
  public void ctor_it_should_create_a_new_One_Time_Password_with_parameters()
  {
    TenantId tenantId = new(Guid.NewGuid().ToString());
    ActorId actorId = ActorId.NewId();
    DateTime expiresOn = DateTime.Now.AddHours(1);
    int maximumAttempts = 5;
    OneTimePasswordId id = new(Guid.NewGuid().ToString());

    OneTimePasswordAggregate oneTimePassword = new(_password, tenantId, expiresOn, maximumAttempts, actorId, id);
    AssertPassword(oneTimePassword, PasswordString);

    Assert.Equal(id, oneTimePassword.Id);
    Assert.Equal(actorId, oneTimePassword.CreatedBy);
    Assert.Equal(expiresOn, oneTimePassword.ExpiresOn);
    Assert.Equal(maximumAttempts, oneTimePassword.MaximumAttempts);
    Assert.Equal(tenantId, oneTimePassword.TenantId);
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the expiration is not in the future.")]
  public void ctor_it_should_throw_ValidationException_when_the_expiration_is_not_in_the_future()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new OneTimePasswordAggregate(_password, expiresOn: DateTime.Now.AddMinutes(-1)));
    Assert.Contains(exception.Errors, e => e.ErrorCode == nameof(FutureValidator));
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the maximum attempts are lesser or equal to 0.")]
  public void ctor_it_should_throw_ValidationException_when_the_maximum_attempts_are_lesser_or_equal_to_0()
  {
    FluentValidation.ValidationException exception;

    exception = Assert.Throws<FluentValidation.ValidationException>(() => new OneTimePasswordAggregate(_password, maximumAttempts: 0));
    Assert.Contains(exception.Errors, e => e.ErrorCode == nameof(MaximumAttemptsValidator));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => new OneTimePasswordAggregate(_password, maximumAttempts: -1));
    Assert.Contains(exception.Errors, e => e.ErrorCode == nameof(MaximumAttemptsValidator));
  }

  [Fact(DisplayName = "Delete: it should delete the One-Time Password when it is not deleted.")]
  public void Delete_it_should_delete_the_One_Time_Password_when_it_is_not_deleted()
  {
    Assert.False(_oneTimePassword.IsDeleted);

    _oneTimePassword.Delete();
    Assert.True(_oneTimePassword.IsDeleted);
    Assert.Contains(_oneTimePassword.Changes, change => change is OneTimePasswordDeletedEvent);

    _oneTimePassword.ClearChanges();
    _oneTimePassword.Delete();
    Assert.False(_oneTimePassword.HasChanges);
  }

  [Fact(DisplayName = "IsExpired: it should return false when the One-Time Password has no expiration.")]
  public void IsExpired_it_should_return_false_when_the_One_Time_Password_has_no_expiration()
  {
    Assert.Null(_oneTimePassword.ExpiresOn);
    Assert.False(_oneTimePassword.IsExpired());
  }

  [Fact(DisplayName = "IsExpired: it should return false when the One-Time Password is not expired.")]
  public void IsExpired_it_should_return_false_when_the_One_Time_Password_is_not_expired()
  {
    DateTime now = DateTime.Now;
    OneTimePasswordAggregate oneTimePassword = new(_password, expiresOn: now.AddHours(1));

    Assert.False(oneTimePassword.IsExpired(now.AddMinutes(50)));
  }

  [Fact(DisplayName = "IsExpired: it should return true when the One-Time Password is expired.")]
  public void IsExpired_it_should_return_true_when_the_One_Time_Password_is_expired()
  {
    DateTime now = DateTime.Now;
    OneTimePasswordAggregate oneTimePassword = new(_password, expiresOn: now.AddHours(1));

    Assert.True(oneTimePassword.IsExpired(now.AddMinutes(70)));
  }

  [Fact(DisplayName = "RemoveCustomAttribute: it should remove an existing custom attribute.")]
  public void RemoveCustomAttribute_it_should_remove_an_existing_custom_attribute()
  {
    string key = "Purpose";

    _oneTimePassword.SetCustomAttribute(key, "MFA");
    _oneTimePassword.Update();

    _oneTimePassword.RemoveCustomAttribute($"  {key}  ");
    _oneTimePassword.Update();
    Assert.False(_oneTimePassword.CustomAttributes.ContainsKey(key));

    _oneTimePassword.RemoveCustomAttribute(key);
    AssertHasNoUpdate(_oneTimePassword);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should set a custom attribute when it is different.")]
  public void SetCustomAttribute_it_should_set_a_custom_attribute_when_it_is_different()
  {
    string key = "  Purpose  ";
    string value = "  MFA  ";
    _oneTimePassword.SetCustomAttribute(key, value);
    Assert.Equal(value.Trim(), _oneTimePassword.CustomAttributes[key.Trim()]);

    _oneTimePassword.Update();

    _oneTimePassword.SetCustomAttribute(key, value);
    AssertHasNoUpdate(_oneTimePassword);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should throw ValidationException when the key or value is not valid.")]
  public void SetCustomAttribute_it_should_throw_ValidationException_when_the_key_or_value_is_not_valid()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _oneTimePassword.SetCustomAttribute("   ", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    string key = _faker.Random.String(IdentifierValidator.MaximumLength + 1, minChar: 'A', maxChar: 'Z');
    exception = Assert.Throws<FluentValidation.ValidationException>(() => _oneTimePassword.SetCustomAttribute(key, "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _oneTimePassword.SetCustomAttribute("-key", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _oneTimePassword.SetCustomAttribute("key", "     "));
    Assert.All(exception.Errors, e => Assert.Equal("Value", e.PropertyName));
  }

  [Fact(DisplayName = "Update: it should update the One-Time Password when it has changes.")]
  public void Update_it_should_update_the_One_Time_Password_when_it_has_changes()
  {
    ActorId actorId = ActorId.NewId();

    _oneTimePassword.SetCustomAttribute("Purpose", "MFA");
    _oneTimePassword.Update(actorId);
    Assert.Equal(actorId, _oneTimePassword.UpdatedBy);

    long version = _oneTimePassword.Version;
    _oneTimePassword.Update(actorId);
    Assert.Equal(version, _oneTimePassword.Version);
  }

  [Fact(DisplayName = "Validate: it should handle validation failure correctly.")]
  public void Validate_it_should_handle_validation_failure_correctly()
  {
    int attemptCount = _oneTimePassword.AttemptCount;
    string incorrectPassword = new(PasswordString.Reverse().ToArray());
    ActorId actorId = ActorId.NewId();

    var exception = Assert.Throws<IncorrectOneTimePasswordPasswordException>(() => _oneTimePassword.Validate(incorrectPassword, actorId));
    Assert.Equal(_oneTimePassword.Id, exception.OneTimePasswordId);
    Assert.Equal(incorrectPassword, exception.AttemptedPassword);

    Assert.Equal(attemptCount + 1, _oneTimePassword.AttemptCount);

    Assert.Contains(_oneTimePassword.Changes, change => change is OneTimePasswordValidationFailedEvent && change.ActorId == actorId);
  }

  [Fact(DisplayName = "Validate: it should handle validation success correctly.")]
  public void Validate_it_should_handle_validation_success_correctly()
  {
    int attemptCount = _oneTimePassword.AttemptCount;
    ActorId actorId = ActorId.NewId();

    _oneTimePassword.Validate(PasswordString, actorId);
    Assert.Equal(attemptCount + 1, _oneTimePassword.AttemptCount);
    Assert.True(_oneTimePassword.HasValidationSucceeded);

    Assert.Contains(_oneTimePassword.Changes, change => change is OneTimePasswordValidationSucceededEvent && change.ActorId == actorId);
  }

  [Fact(DisplayName = "Validate: it should throw OneTimePasswordAlreadyUsedException when the One-Time Password has already been used.")]
  public void Validate_it_should_throw_OneTimePasswordAlreadyUsedException_when_the_One_Time_Password_has_already_been_used()
  {
    _oneTimePassword.Validate(PasswordString);

    var exception = Assert.Throws<OneTimePasswordAlreadyUsedException>(() => _oneTimePassword.Validate(PasswordString));
    Assert.Equal(_oneTimePassword.Id, exception.OneTimePasswordId);
  }

  [Fact(DisplayName = "Validate: it should throw MaximumAttemptsReachedException when the maximum number of attempts has been reached.")]
  public void Validate_it_should_throw_MaximumAttemptsReachedException_when_the_maximum_number_of_attempts_has_been_reached()
  {
    OneTimePasswordAggregate oneTimePassword = new(_password, maximumAttempts: 1);
    string incorrectPassword = new(PasswordString.Reverse().ToArray());
    try
    {
      oneTimePassword.Validate(incorrectPassword);
    }
    catch (IncorrectOneTimePasswordPasswordException)
    {
    }

    var exception = Assert.Throws<MaximumAttemptsReachedException>(() => oneTimePassword.Validate(PasswordString));
    Assert.Equal(oneTimePassword.Id, exception.OneTimePasswordId);
    Assert.Equal(oneTimePassword.AttemptCount, exception.AttemptCount);
  }

  [Fact(DisplayName = "Validate: it should throw OneTimePasswordIsExpiredException when the One-Time Password is expired.")]
  public void Validate_it_should_throw_OneTimePasswordIsExpiredException_when_the_One_Time_Password_is_expired()
  {
    OneTimePasswordAggregate oneTimePassword = new(_password, expiresOn: DateTime.Now.AddMilliseconds(50));

    Thread.Sleep(100);

    var exception = Assert.Throws<OneTimePasswordIsExpiredException>(() => oneTimePassword.Validate(PasswordString));
    Assert.Equal(oneTimePassword.Id, exception.OneTimePasswordId);
  }

  private static void AssertHasNoUpdate(OneTimePasswordAggregate oneTimePassword)
  {
    FieldInfo? field = oneTimePassword.GetType().GetField("_updatedEvent", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    OneTimePasswordUpdatedEvent? updated = field.GetValue(oneTimePassword) as OneTimePasswordUpdatedEvent;
    Assert.NotNull(updated);
    Assert.False(updated.HasChanges);
  }
  private static void AssertPassword(OneTimePasswordAggregate oneTimePassword, string? password)
  {
    FieldInfo? field = oneTimePassword.GetType().GetField("_password", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    Password? instance = field.GetValue(oneTimePassword) as Password;
    if (password == null)
    {
      Assert.Null(instance);
    }
    else
    {
      Assert.NotNull(instance);
      Assert.True(instance.IsMatch(password));
    }
  }
}
