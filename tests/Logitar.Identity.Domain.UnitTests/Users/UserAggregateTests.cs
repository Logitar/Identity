using Bogus;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

[Trait(Traits.Category, Categories.Unit)]
public class UserAggregateTests
{
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

  [Fact(DisplayName = "ctor: it should create a new user with id.")]
  public void ctor_it_should_create_a_new_user_with_id()
  {
    UserId userId = new(AggregateId.NewId());

    UserAggregate user = new(userId.AggregateId);

    Assert.Equal(userId, user.Id);
    Assert.Equal(userId.AggregateId, user.Id.AggregateId);
    Assert.Null(user.TenantId);
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
    Assert.Null(exception.PropertyName);
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

    string key = _faker.Random.String(CustomAttributeKeyValidator.MaximumLength + 1, minChar: 'A', maxChar: 'Z');
    exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomAttribute(key, "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomAttribute("-key", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _user.SetCustomAttribute("key", "     "));
    Assert.All(exception.Errors, e => Assert.Equal("Value", e.PropertyName));
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
    UserAggregate user = new(AggregateId.NewId());
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

  private static void AssertHasNoUpdate(UserAggregate user)
  {
    FieldInfo? field = user.GetType().GetField("_updated", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    UserUpdatedEvent? updated = field.GetValue(user) as UserUpdatedEvent;
    Assert.NotNull(updated);
    Assert.False(updated.HasChanges);
  }
}
