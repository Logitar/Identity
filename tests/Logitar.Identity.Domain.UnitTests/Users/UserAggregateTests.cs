using Bogus;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;
using System.Reflection;

namespace Logitar.Identity.Domain.Users;

[Trait(Traits.Category, Categories.Unit)]
public class UserAggregateTests
{
  private readonly Faker _faker = new();
  private readonly UniqueNameSettings _uniqueNameSettings = new();
  private readonly UniqueNameUnit _uniqueName;
  private readonly UserAggregate _user;

  public UserAggregateTests()
  {
    _uniqueName = new(_uniqueNameSettings, "admin");
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

  [Fact(DisplayName = "Description: it should change the description when it is different.")]
  public void Description_it_should_change_the_description_when_it_is_different()
  {
    DescriptionUnit description = new("This is the main administration user.");
    _user.Description = description;
    Assert.Equal(description, _user.Description);

    _user.Update();

    _user.Description = description;
    AssertHasNoUpdate(_user);
  }

  [Fact(DisplayName = "DisplayName: it should change the display name when it is different.")]
  public void DisplayName_it_should_change_the_display_name_when_it_is_different()
  {
    DisplayNameUnit displayName = new("Administrator");
    _user.DisplayName = displayName;
    Assert.Equal(displayName, _user.DisplayName);

    _user.Update();

    _user.DisplayName = displayName;
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
    UniqueNameUnit uniqueName = new(_uniqueNameSettings, "manage_users");
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

    DisplayNameUnit displayName = new("Administrator");
    _user.DisplayName = displayName;
    Assert.StartsWith($"{displayName.Value} | ", _user.ToString());
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

    _user.DisplayName = new DisplayNameUnit("Administrator");
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
