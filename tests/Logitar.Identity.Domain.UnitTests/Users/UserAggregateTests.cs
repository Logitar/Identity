using Bogus;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Domain.Users;

[Trait(Traits.Category, Categories.Unit)]
public class UserAggregateTests
{
  private const string PasswordString = "Test123!";

  private readonly Faker _faker = new();
  private readonly UniqueNameSettings _uniqueNameSettings = new();
  private readonly UniqueNameUnit _uniqueName;
  private readonly RoleAggregate _role;
  private readonly UserAggregate _user;

  public UserAggregateTests()
  {
    _uniqueName = new(_uniqueNameSettings, _faker.Person.UserName);
    _role = new(new UniqueNameUnit(_uniqueNameSettings, "admin"));
    _user = new(_uniqueName);
  }

  [Fact(DisplayName = "AddRole: it should add the role to the user when it does not have the role.")]
  public void AddRole_it_should_add_the_role_to_the_user_when_it_does_not_have_the_role()
  {
    Assert.False(_user.HasRole(_role));
    Assert.Empty(_user.Roles);

    _user.AddRole(_role);
    Assert.Contains(_user.Changes, changes => changes is UserRoleAddedEvent @event && @event.RoleId == _role.Id);
    Assert.True(_user.HasRole(_role));
    Assert.Single(_user.Roles, _role.Id);

    _user.ClearChanges();
    _user.AddRole(_role);
    Assert.False(_user.HasChanges);
  }

  [Fact(DisplayName = "AddRole: it should throw TenantMismatchException when the role is in a different tenant.")]
  public void AddRole_it_should_throw_TenantMismatchException_when_the_role_is_in_a_different_tenant()
  {
    TenantId tenantId = new(Guid.NewGuid().ToString());
    RoleAggregate role = new(_role.UniqueName, tenantId);

    var exception = Assert.Throws<TenantMismatchException>(() => _user.AddRole(role));
    Assert.Equal(_user.TenantId, exception.ExpectedTenantId);
    Assert.Equal(role.TenantId, exception.ActualTenantId);
  }

  [Fact(DisplayName = "Authenticate: it should authenticate the user.")]
  public void Authenticate_it_should_authenticate_the_user()
  {
    Base64Password password = new(PasswordString);
    _user.SetPassword(password);

    _user.Authenticate(PasswordString);

    UserAuthenticatedEvent @event = (UserAuthenticatedEvent)Assert.Single(_user.Changes, change => change is UserAuthenticatedEvent);
    Assert.Equal(_user.Id.Value, @event.ActorId.Value);
    Assert.Equal(@event.OccurredOn, _user.AuthenticatedOn);
  }

  [Fact(DisplayName = "Authenticate: it should authenticate the user using the specified actor identifier.")]
  public void Authenticate_it_should_authenticate_the_user_using_the_specified_actor_identifier()
  {
    Base64Password password = new(PasswordString);
    _user.SetPassword(password);

    ActorId actorId = ActorId.NewId();
    _user.Authenticate(PasswordString, actorId);
    Assert.Contains(_user.Changes, change => change is UserAuthenticatedEvent @event && @event.ActorId == actorId);
  }

  [Fact(DisplayName = "Authenticate: it should throw IncorrectUserPasswordException when the attempted password is incorrect.")]
  public void Authenticate_it_should_throw_IncorrectUserPasswordException_when_the_attempted_password_is_incorrect()
  {
    Base64Password password = new(PasswordString[1..]);
    _user.SetPassword(password);

    var exception = Assert.Throws<IncorrectUserPasswordException>(() => _user.Authenticate(PasswordString));
    Assert.Equal(PasswordString, exception.AttemptedPassword);
    Assert.Equal(_user.Id, exception.UserId);
  }

  [Fact(DisplayName = "Authenticate: it should throw UserHasNoPasswordException when the user has no password.")]
  public void Authenticate_it_should_throw_UserHasNoPasswordException_when_the_user_has_no_password()
  {
    var exception = Assert.Throws<UserHasNoPasswordException>(() => _user.Authenticate(PasswordString));
    Assert.Equal(_user.Id, exception.UserId);
  }

  [Fact(DisplayName = "Authenticate: it should throw UserIsDisabledException when the user is disabled.")]
  public void Authenticate_it_should_throw_UserIsDisabledException_when_the_user_is_disabled()
  {
    Base64Password password = new(PasswordString);
    _user.SetPassword(password);
    _user.Disable();

    var exception = Assert.Throws<UserIsDisabledException>(() => _user.Authenticate(PasswordString));
    Assert.Equal(_user.Id, exception.UserId);
  }

  [Fact(DisplayName = "Birthdate: it should change the birthdate when it is different.")]
  public void Birthdate_it_should_change_the_birthdate_when_it_is_different()
  {
    DateTime birthdate = _faker.Person.DateOfBirth;

    _user.Birthdate = birthdate;
    Assert.Equal(birthdate, _user.Birthdate);

    _user.Update();

    _user.Birthdate = birthdate;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "Birthdate: it should throw ValidationException when the value is in the future.")]
  public void Birthdate_it_should_throw_ValidationException_when_the_value_is_in_the_future()
  {
    DateTime birthdate = DateTime.Now.AddYears(1);
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.Birthdate = birthdate);
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal("PastValidator", e.ErrorCode);
      Assert.Equal(nameof(_user.Birthdate), e.PropertyName);
    });
  }

  [Fact(DisplayName = "ChangePassword: it should change the users password.")]
  public void ChangePassword_it_should_change_the_users_password()
  {
    string oldPassword = PasswordString[1..];
    _user.SetPassword(new Base64Password(oldPassword));
    _user.ClearChanges();
    AssertPassword(_user, oldPassword);

    Base64Password newPassword = new(PasswordString);
    _user.ChangePassword(oldPassword, newPassword);
    AssertPassword(_user, PasswordString);
    Assert.Contains(_user.Changes, change => change is UserPasswordChangedEvent @event
      && @event.ActorId.Value == _user.Id.Value
      && @event.Password.IsMatch(PasswordString));
  }

  [Fact(DisplayName = "ChangePassword: it should change the users password using the specified actor identifier.")]
  public void ChangePassword_it_should_change_the_users_password_using_the_specified_actor_identifier()
  {
    string oldPassword = PasswordString[1..];
    _user.SetPassword(new Base64Password(oldPassword));
    _user.ClearChanges();
    AssertPassword(_user, oldPassword);

    Base64Password newPassword = new(PasswordString);
    ActorId actorId = ActorId.NewId();
    _user.ChangePassword(oldPassword, newPassword, actorId);
    AssertPassword(_user, PasswordString);
    Assert.Contains(_user.Changes, change => change is UserPasswordChangedEvent @event && @event.ActorId == actorId);
  }

  [Fact(DisplayName = "ChangePassword: it should throw IncorrectUserPasswordException when the attempted password is incorrect.")]
  public void ChangePassword_it_should_throw_IncorrectUserPasswordException_when_the_attempted_password_is_incorrect()
  {
    _user.SetPassword(new Base64Password(PasswordString));
    AssertPassword(_user, PasswordString);

    Base64Password password = new(PasswordString);
    string attemptedPassword = PasswordString[1..];
    var exception = Assert.Throws<IncorrectUserPasswordException>(() => _user.ChangePassword(attemptedPassword, password));
    Assert.Equal(attemptedPassword, exception.AttemptedPassword);
    Assert.Equal(_user.Id, exception.UserId);
  }

  [Fact(DisplayName = "ChangePassword: it should throw UserHasNoPasswordException when the user has no password.")]
  public void ChangePassword_it_should_throw_UserHasNoPasswordException_when_the_user_has_no_password()
  {
    AssertPassword(_user, password: null);

    Base64Password password = new(PasswordString);
    var exception = Assert.Throws<UserHasNoPasswordException>(() => _user.ChangePassword(PasswordString, password));
    Assert.Equal(_user.Id, exception.UserId);
  }

  [Fact(DisplayName = "ChangePassword: it should throw UserIsDisabledException when the user is disabled.")]
  public void ChangePassword_it_should_throw_UserIsDisabledException_when_the_user_is_disabled()
  {
    Base64Password password = new(PasswordString);
    _user.SetPassword(password);

    _user.Disable();

    var exception = Assert.Throws<UserIsDisabledException>(() => _user.ChangePassword(PasswordString, password));
    Assert.Equal(_user.Id, exception.UserId);
  }

  [Fact(DisplayName = "ctor: it should create a new user with parameters.")]
  public void ctor_it_should_create_a_new_user_with_parameters()
  {
    TenantId tenantId = new(Guid.NewGuid().ToString());
    ActorId actorId = ActorId.NewId();
    UserId id = new(Guid.NewGuid().ToString());

    UserAggregate user = new(_uniqueName, tenantId, actorId, id);

    Assert.Equal(id, user.Id);
    Assert.Equal(actorId, user.CreatedBy);
    Assert.Equal(tenantId, user.TenantId);
    Assert.Equal(_uniqueName, user.UniqueName);
  }

  [Fact(DisplayName = "Delete: it should delete the user when it is not deleted.")]
  public void Delete_it_should_delete_the_user_when_it_is_not_deleted()
  {
    Assert.False(_user.IsDeleted);

    _user.Delete();
    Assert.True(_user.IsDeleted);
    Assert.Contains(_user.Changes, change => change is UserDeletedEvent);

    _user.ClearChanges();
    _user.Delete();
    Assert.False(_user.HasChanges);
  }

  [Fact(DisplayName = "Disable: it should disable the user when it is enabled.")]
  public void Disable_it_should_disable_the_user_when_it_is_enabled()
  {
    Assert.False(_user.IsDisabled);

    _user.Disable();
    Assert.True(_user.IsDisabled);
    Assert.Contains(_user.Changes, change => change is UserDisabledEvent);

    _user.ClearChanges();
    _user.Disable();
    Assert.False(_user.HasChanges);
  }

  [Fact(DisplayName = "Enable: it should enable the user when it is disabled.")]
  public void Enable_it_should_enable_the_user_when_it_is_disabled()
  {
    _user.Disable();
    Assert.True(_user.IsDisabled);

    _user.ClearChanges();

    _user.Enable();
    Assert.False(_user.IsDisabled);
    Assert.Contains(_user.Changes, change => change is UserEnabledEvent);

    _user.ClearChanges();
    _user.Enable();
    Assert.False(_user.HasChanges);
  }

  [Fact(DisplayName = "FirstName: it should change the first name when it is different.")]
  public void FirstName_it_should_change_the_first_name_when_it_is_different()
  {
    PersonNameUnit firstName = new(_faker.Person.FirstName);
    _user.FirstName = firstName;
    Assert.Equal(firstName, _user.FirstName);

    _user.Update();

    _user.FirstName = firstName;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "Gender: it should change the gender when it is different.")]
  public void Gender_it_should_change_the_gender_when_it_is_different()
  {
    GenderUnit gender = new(_faker.Person.Gender.ToString());

    _user.Gender = gender;
    Assert.Equal(gender, _user.Gender);

    _user.Update();

    _user.Gender = gender;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "HasPassword: it should return false when the user has no password.")]
  public void HasPassword_it_should_return_false_when_the_user_has_no_password()
  {
    AssertPassword(_user, password: null);
    Assert.False(_user.HasPassword);
  }

  [Fact(DisplayName = "HasPassword: it should return true when the user has a password.")]
  public void HasPassword_it_should_return_true_when_the_user_has_a_password()
  {
    Base64Password password = new(PasswordString);
    _user.SetPassword(password);
    Assert.True(_user.HasPassword);
  }

  [Fact(DisplayName = "HasRole: it should return false when the Api key does not have the specified role.")]
  public void HasRole_it_should_return_false_when_the_Api_key_does_not_have_the_specified_role()
  {
    Assert.False(_user.HasRole(_role));
  }

  [Fact(DisplayName = "HasRole: it should return true when the Api key does have the specified role.")]
  public void HasRole_it_should_return_true_when_the_Api_key_does_have_the_specified_role()
  {
    _user.AddRole(_role);
    Assert.True(_user.HasRole(_role));
  }

  [Fact(DisplayName = "IsConfirmed: it should be false when the user has no verified contact information.")]
  public void IsConfirmed_it_should_be_false_when_the_user_has_no_verified_contact_information()
  {
    _user.SetAddress(new AddressUnit("150 Saint-Catherine St W", "Montreal", "CA", "QC", "H2X 3Y2", isVerified: false));
    _user.SetEmail(new EmailUnit(_faker.Person.Email, isVerified: false));
    _user.SetPhone(new PhoneUnit("+15148454636", "CA", "12345", isVerified: false));
    Assert.False(_user.IsConfirmed);
  }

  [Fact(DisplayName = "IsConfirmed: it should be true when the user has at least one verified contact information.")]
  public void IsConfirmed_it_should_be_true_when_the_user_has_at_least_one_verified_contact_information()
  {
    _user.SetAddress(new AddressUnit("150 Saint-Catherine St W", "Montreal", "CA", "QC", "H2X 3Y2", isVerified: false));
    _user.SetEmail(new EmailUnit(_faker.Person.Email, isVerified: true));
    _user.SetPhone(new PhoneUnit("+15148454636", "CA", "12345", isVerified: false));
    Assert.True(_user.IsConfirmed);
  }

  [Fact(DisplayName = "LastName: it should change the last name when it is different.")]
  public void LastName_it_should_change_the_last_name_when_it_is_different()
  {
    PersonNameUnit lastName = new(_faker.Person.LastName);
    _user.LastName = lastName;
    Assert.Equal(lastName, _user.LastName);

    _user.Update();

    _user.LastName = lastName;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "Locale: it should change the locale when it is different.")]
  public void Locale_it_should_change_the_locale_when_it_is_different()
  {
    LocaleUnit locale = new(_faker.Locale);

    _user.Locale = locale;
    Assert.Equal(locale, _user.Locale);

    _user.Update();

    _user.Locale = locale;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "MiddleName: it should change the last name when it is different.")]
  public void MiddleName_it_should_change_the_last_name_when_it_is_different()
  {
    PersonNameUnit middleName = new(_faker.Name.FirstName(_faker.Person.Gender));
    _user.MiddleName = middleName;
    Assert.Equal(middleName, _user.MiddleName);

    _user.Update();

    _user.MiddleName = middleName;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "Nickname: it should change the last name when it is different.")]
  public void Nickname_it_should_change_the_last_name_when_it_is_different()
  {
    PersonNameUnit nickname = new(string.Concat(_faker.Person.FirstName.First(), _faker.Person.LastName).ToLower());
    _user.Nickname = nickname;
    Assert.Equal(nickname, _user.Nickname);

    _user.Update();

    _user.Nickname = nickname;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "Picture: it should change the picture when it is different.")]
  public void Picture_it_should_change_the_picture_when_it_is_different()
  {
    UrlUnit picture = new(_faker.Person.Avatar);
    _user.Picture = picture;
    Assert.Equal(picture, _user.Picture);

    _user.Update();

    _user.Picture = picture;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "Profile: it should change the profile page when it is different.")]
  public void Profile_it_should_change_the_profile_page_when_it_is_different()
  {
    UrlUnit profile = new($"https://www.test.com/employees/{_user.UniqueName.Value}");
    _user.Profile = profile;
    Assert.Equal(profile, _user.Profile);

    _user.Update();

    _user.Profile = profile;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "RemoveCustomAttribute: it should remove an existing custom attribute.")]
  public void RemoveCustomAttribute_it_should_remove_an_existing_custom_attribute()
  {
    string key = "remove_users";

    _user.SetCustomAttribute(key, bool.TrueString);
    _user.Update();

    _user.RemoveCustomAttribute($"  {key}  ");
    _user.Update();
    Assert.False(_user.CustomAttributes.ContainsKey(key));

    _user.RemoveCustomAttribute(key);
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "RemoveCustomIdentifier: it should remove an existing custom identifier.")]
  public void RemoveCustomIdentifier_it_should_remove_an_existing_custom_identifier()
  {
    string key = "google_id";

    _user.SetCustomIdentifier(key, bool.TrueString);
    _user.Update();

    _user.RemoveCustomIdentifier($"  {key}  ");
    _user.Update();
    Assert.False(_user.CustomIdentifiers.ContainsKey(key));

    _user.RemoveCustomIdentifier(key);
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "RemoveRole: it should remove the role from the user when it has the role.")]
  public void RemoveRole_it_should_remove_the_role_from_the_user_when_it_has_the_role()
  {
    _user.AddRole(_role);
    Assert.True(_user.HasRole(_role));
    Assert.Single(_user.Roles, _role.Id);
    _user.ClearChanges();

    _user.RemoveRole(_role);
    Assert.Contains(_user.Changes, changes => changes is UserRoleRemovedEvent @event && @event.RoleId == _role.Id);
    Assert.False(_user.HasRole(_role));
    Assert.Empty(_user.Roles);

    _user.ClearChanges();
    _user.RemoveRole(_role);
    Assert.False(_user.HasChanges);
  }

  [Fact(DisplayName = "ResetPassword: it should reset the user's password.")]
  public void ResetPassword_it_should_reset_the_users_password()
  {
    Base64Password password = new(PasswordString);
    _user.ResetPassword(password);
    AssertPassword(_user, PasswordString);
    Assert.Contains(_user.Changes, change => change is UserPasswordResetEvent @event
      && @event.ActorId.Value == _user.Id.Value
      && @event.Password.IsMatch(PasswordString));
  }

  [Fact(DisplayName = "ResetPassword: it should reset the user's password using the specified actor identifier.")]
  public void ResetPassword_it_should_reset_the_users_password_using_the_specified_actor_identifier()
  {
    Base64Password password = new(PasswordString);
    ActorId actorId = ActorId.NewId();
    _user.ResetPassword(password, actorId);
    Assert.Contains(_user.Changes, change => change is UserPasswordResetEvent @event && @event.ActorId == actorId);
  }

  [Fact(DisplayName = "ResetPassword: it should throw UserIsDisabledException when the user is disabled.")]
  public void ResetPassword_it_should_throw_UserIsDisabledException_when_the_user_is_disabled()
  {
    _user.Disable();

    Base64Password password = new(PasswordString);
    var exception = Assert.Throws<UserIsDisabledException>(() => _user.ResetPassword(password));
    Assert.Equal(_user.Id, exception.UserId);
  }

  [Fact(DisplayName = "SetAddress: it should change the postal address when it is different.")]
  public void SetAddress_it_should_change_the_postal_address_when_it_is_different()
  {
    AddressUnit address = new("150 Saint-Catherine St W", "Montreal", "CA", "QC", "H2X 3Y2");
    _user.SetAddress(address);
    Assert.Equal(address, _user.Address);

    _user.Update();

    _user.SetAddress(address);
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should set a custom attribute when it is different.")]
  public void SetCustomAttribute_it_should_set_a_custom_attribute_when_it_is_different()
  {
    string key = "  remove_users  ";
    string value = $"  {bool.TrueString}  ";
    _user.SetCustomAttribute(key, value);
    Assert.Equal(value.Trim(), _user.CustomAttributes[key.Trim()]);

    _user.Update();

    _user.SetCustomAttribute(key, value);
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should throw ValidationException when the key or value is not valid.")]
  public void SetCustomAttribute_it_should_throw_ValidationException_when_the_key_or_value_is_not_valid()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomAttribute("   ", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    string key = _faker.Random.String(IdentifierValidator.MaximumLength + 1, minChar: 'A', maxChar: 'Z');
    exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomAttribute(key, "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomAttribute("-key", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomAttribute("key", "     "));
    Assert.All(exception.Errors, e => Assert.Equal("Value", e.PropertyName));
  }

  [Fact(DisplayName = "SetCustomIdentifier: it should set a custom identifier when it is different.")]
  public void SetCustomIdentifier_it_should_set_a_custom_identifier_when_it_is_different()
  {
    string key = "  google_id  ";
    string value = $"  {Guid.NewGuid()}  ";
    _user.SetCustomIdentifier(key, value);
    Assert.Equal(value.Trim(), _user.CustomIdentifiers[key.Trim()]);

    _user.Update();

    _user.SetCustomIdentifier(key, value);
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "SetCustomIdentifier: it should throw ValidationException when the key or value is not valid.")]
  public void SetCustomIdentifier_it_should_throw_ValidationException_when_the_key_or_value_is_not_valid()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomIdentifier("   ", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    string key = _faker.Random.String(IdentifierValidator.MaximumLength + 1, minChar: 'A', maxChar: 'Z');
    exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomIdentifier(key, "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomIdentifier("-key", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomIdentifier("key", "     "));
    Assert.All(exception.Errors, e => Assert.Equal("Value", e.PropertyName));
  }

  [Fact(DisplayName = "SetEmail: it should change the email address when it is different.")]
  public void SetEmail_it_should_change_the_email_address_when_it_is_different()
  {
    EmailUnit email = new(_faker.Person.Email);
    _user.SetEmail(email);
    Assert.Equal(email, _user.Email);

    _user.Update();

    _user.SetEmail(email);
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "SetPassword: it should change the password.")]
  public void SetPassword_it_should_change_the_password()
  {
    AssertPassword(_user, password: null);

    ActorId actorId = ActorId.NewId();
    Base64Password password = new(PasswordString);
    _user.SetPassword(password, actorId);

    AssertPassword(_user, PasswordString);
    Assert.Contains(_user.Changes, change => change is UserPasswordUpdatedEvent @event
      && @event.ActorId == actorId
      && @event.Password.IsMatch(PasswordString));
  }

  [Fact(DisplayName = "SetPhone: it should change the phone number when it is different.")]
  public void SetPhone_it_should_change_the_phone_number_when_it_is_different()
  {
    PhoneUnit phone = new("+15148454636", "CA", "12345");
    _user.SetPhone(phone);
    Assert.Equal(phone, _user.Phone);

    _user.Update();

    _user.SetPhone(phone);
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "SetUniqueName: it should change the unique name when it is different.")]
  public void SetUniqueName_it_should_change_the_unique_name_when_it_is_different()
  {
    UniqueNameUnit uniqueName = new(_uniqueNameSettings, _faker.Internet.UserName());
    _user.SetUniqueName(uniqueName);
    Assert.Equal(uniqueName, _user.UniqueName);
    Assert.Contains(_user.Changes, change => change is UserUniqueNameChangedEvent);

    _user.ClearChanges();
    _user.SetUniqueName(uniqueName);
    Assert.False(_user.HasChanges);
  }

  [Fact(DisplayName = "SignIn: it should open a new session with a password check.")]
  public void SignIn_it_should_open_a_new_session_with_a_password_check()
  {
    Base64Password password = new(PasswordString);
    _user.SetPassword(password);

    string secretString = RandomStringGenerator.GetString(32);
    Base64Password secret = new(secretString);
    ActorId actorId = ActorId.NewId();
    SessionId sessionId = new(Guid.NewGuid().ToString());
    SessionAggregate session = _user.SignIn(PasswordString, secret, actorId, sessionId);
    Assert.Equal(_user.Id, session.UserId);
    Assert.True(session.IsActive);
    Assert.True(session.IsPersistent);

    Assert.Equal(sessionId, session.Id);
    Assert.Equal(actorId, session.CreatedBy);
    AssertSecret(session, secretString);

    Assert.Contains(_user.Changes, change => change is UserSignedInEvent @event
      && @event.ActorId == actorId
      && @event.OccurredOn == session.CreatedOn);
  }

  [Fact(DisplayName = "SignIn: it should open a new session without a password check.")]
  public void SignIn_it_should_open_a_new_session_without_a_password_check()
  {
    SessionAggregate session = _user.SignIn();
    Assert.Equal(_user.Id, session.UserId);
    Assert.True(session.IsActive);
    Assert.False(session.IsPersistent);

    Assert.Equal(_user.Id.Value, session.CreatedBy.Value);
    Assert.Contains(_user.Changes, change => change is UserSignedInEvent @event
      && @event.ActorId.Value == _user.Id.Value
      && @event.OccurredOn == session.CreatedOn);
  }

  [Fact(DisplayName = "SignIn: it should throw IncorrectUserPasswordException when the password is incorrect.")]
  public void SignIn_it_should_throw_IncorrectUserPasswordException_when_the_password_is_incorrect()
  {
    Base64Password password = new(PasswordString[1..]);
    _user.SetPassword(password);

    var exception = Assert.Throws<IncorrectUserPasswordException>(() => _user.SignIn(PasswordString));
    Assert.Equal(PasswordString, exception.AttemptedPassword);
    Assert.Equal(_user.Id, exception.UserId);
  }

  [Fact(DisplayName = "SignIn: it should throw UserHasNoPasswordException when the user has no password.")]
  public void SignIn_it_should_throw_UserHasNoPasswordException_when_the_user_has_no_password()
  {
    var exception = Assert.Throws<UserHasNoPasswordException>(() => _user.SignIn(PasswordString));
    Assert.Equal(_user.Id, exception.UserId);
  }

  [Fact(DisplayName = "SignIn: it should throw UserIsDisabledException when the user is disabled.")]
  public void SignIn_it_should_throw_UserIsDisabledException_when_the_user_is_disabled()
  {
    _user.Disable();

    var exception = Assert.Throws<UserIsDisabledException>(() => _user.SignIn());
    Assert.Equal(_user.Id, exception.UserId);
  }

  [Fact(DisplayName = "TimeZone: it should change the time zone when it is different.")]
  public void TimeZone_it_should_change_the_time_zone_when_it_is_different()
  {
    TimeZoneUnit timeZone = new("America/New_York");

    _user.TimeZone = timeZone;
    Assert.Equal(timeZone, _user.TimeZone);

    _user.Update();

    _user.TimeZone = timeZone;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "ToString: it should return the correct string representation.")]
  public void ToString_it_should_return_the_correct_string_representation()
  {
    Assert.StartsWith($"{_uniqueName.Value} | ", _user.ToString());

    _user.FirstName = new PersonNameUnit(" Charles-François ");
    _user.LastName = new PersonNameUnit("Henry     Angers");
    Assert.StartsWith("Charles-François Henry Angers | ", _user.ToString());
  }

  [Fact(DisplayName = "UniqueName: it should throw InvalidOperationException when it has not been initialized yet.")]
  public void UniqueName_it_should_throw_InvalidOperationException_when_it_has_not_been_initialized_yet()
  {
    UserAggregate user = new();
    Assert.Throws<InvalidOperationException>(() => _ = user.UniqueName);
  }

  [Fact(DisplayName = "Update: it should update the user when it has changes.")]
  public void Update_it_should_update_the_user_when_it_has_changes()
  {
    ActorId actorId = ActorId.NewId();

    _user.FirstName = new PersonNameUnit(_faker.Person.FirstName);
    _user.LastName = new PersonNameUnit(_faker.Person.LastName);
    _user.Update(actorId);
    Assert.Equal(actorId, _user.UpdatedBy);

    long version = _user.Version;
    _user.Update(actorId);
    Assert.Equal(version, _user.Version);
  }

  [Fact(DisplayName = "Website: it should change the website when it is different.")]
  public void Website_it_should_change_the_website_when_it_is_different()
  {
    UrlUnit website = new($"https://www.{_faker.Person.Website}");
    _user.Website = website;
    Assert.Equal(website, _user.Website);

    _user.Update();

    _user.Website = website;
    AssertHasNoUpdate(_user);
  }

  private static void AssertHasNoUpdate(UserAggregate user)
  {
    FieldInfo? field = user.GetType().GetField("_updatedEvent", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    UserUpdatedEvent? updated = field.GetValue(user) as UserUpdatedEvent;
    Assert.NotNull(updated);
    Assert.False(updated.HasChanges);
  }
  private static void AssertPassword(UserAggregate user, string? password)
  {
    FieldInfo? field = user.GetType().GetField("_password", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    Password? instance = field.GetValue(user) as Password;
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
  private static void AssertSecret(SessionAggregate session, string? secret)
  {
    FieldInfo? field = session.GetType().GetField("_secret", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    Password? instance = field.GetValue(session) as Password;
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
