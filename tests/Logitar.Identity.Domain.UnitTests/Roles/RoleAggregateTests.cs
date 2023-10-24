using Bogus;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Roles;

[Trait(Traits.Category, Categories.Unit)]
public class RoleAggregateTests
{
  private readonly Faker _faker = new();
  private readonly UniqueNameSettings _uniqueNameSettings = new();
  private readonly UniqueNameUnit _uniqueName;
  private readonly RoleAggregate _role;

  public RoleAggregateTests()
  {
    _uniqueName = new(_uniqueNameSettings, "admin");
    _role = new(_uniqueName);
  }

  [Fact(DisplayName = "ctor: it should create a new role with id.")]
  public void ctor_it_should_create_a_new_role_with_id()
  {
    RoleId roleId = new(AggregateId.NewId());

    RoleAggregate role = new(roleId.AggregateId);

    Assert.Equal(roleId, role.Id);
    Assert.Equal(roleId.AggregateId, role.Id.AggregateId);
    Assert.Null(role.TenantId);
  }

  [Fact(DisplayName = "ctor: it should create a new role with parameters.")]
  public void ctor_it_should_create_a_new_role_with_parameters()
  {
    TenantId tenantId = new(Guid.NewGuid().ToString());
    ActorId actorId = ActorId.NewId();
    RoleId id = new(Guid.NewGuid().ToString());

    RoleAggregate role = new(_uniqueName, tenantId, actorId, id);

    Assert.Equal(id, role.Id);
    Assert.Equal(actorId, role.CreatedBy);
    Assert.Equal(tenantId, role.TenantId);
    Assert.Equal(_uniqueName, role.UniqueName);
  }

  [Fact(DisplayName = "Delete: it should delete the role when it is not deleted.")]
  public void Delete_it_should_delete_the_role_when_it_is_not_deleted()
  {
    Assert.False(_role.IsDeleted);

    _role.Delete();
    Assert.True(_role.IsDeleted);
    Assert.Contains(_role.Changes, change => change is RoleDeletedEvent);

    _role.ClearChanges();
    _role.Delete();
    Assert.False(_role.HasChanges);
  }

  [Fact(DisplayName = "Description: it should change the description when it is different.")]
  public void Description_it_should_change_the_description_when_it_is_different()
  {
    DescriptionUnit description = new("This is the main administration role.");
    _role.Description = description;
    Assert.Equal(description, _role.Description);

    _role.Update();

    _role.Description = description;
    AssertHasNoUpdate(_role);
  }

  [Fact(DisplayName = "DisplayName: it should change the display name when it is different.")]
  public void DisplayName_it_should_change_the_display_name_when_it_is_different()
  {
    DisplayNameUnit displayName = new("Administrator");
    _role.DisplayName = displayName;
    Assert.Equal(displayName, _role.DisplayName);

    _role.Update();

    _role.DisplayName = displayName;
    AssertHasNoUpdate(_role);
  }

  [Fact(DisplayName = "RemoveCustomAttribute: it should remove an existing custom attribute.")]
  public void RemoveCustomAttribute_it_should_remove_an_existing_custom_attribute()
  {
    string key = "remove_roles";

    _role.SetCustomAttribute(key, bool.TrueString);
    _role.Update();

    _role.RemoveCustomAttribute($"  {key}  ");
    _role.Update();
    Assert.False(_role.CustomAttributes.ContainsKey(key));

    _role.RemoveCustomAttribute(key);
    AssertHasNoUpdate(_role);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should set a custom attribute when it is different.")]
  public void SetCustomAttribute_it_should_set_a_custom_attribute_when_it_is_different()
  {
    string key = "  remove_roles  ";
    string value = $"  {bool.TrueString}  ";
    _role.SetCustomAttribute(key, value);
    Assert.Equal(value.Trim(), _role.CustomAttributes[key.Trim()]);

    _role.Update();

    _role.SetCustomAttribute(key, value);
    AssertHasNoUpdate(_role);
  }

  [Fact(DisplayName = "SetCustomAttribute: it should throw ValidationException when the key or value is not valid.")]
  public void SetCustomAttribute_it_should_throw_ValidationException_when_the_key_or_value_is_not_valid()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _role.SetCustomAttribute("   ", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    string key = _faker.Random.String(CustomAttributeKeyValidator.MaximumLength + 1, minChar: 'A', maxChar: 'Z');
    exception = Assert.Throws<FluentValidation.ValidationException>(() => _role.SetCustomAttribute(key, "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _role.SetCustomAttribute("-key", "value"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _role.SetCustomAttribute("key", "     "));
    Assert.All(exception.Errors, e => Assert.Equal("Value", e.PropertyName));
  }

  [Fact(DisplayName = "SetUniqueName: it should change the unique name when it is different.")]
  public void SetUniqueName_it_should_change_the_unique_name_when_it_is_different()
  {
    UniqueNameUnit uniqueName = new(_uniqueNameSettings, "manage_users");
    _role.SetUniqueName(uniqueName);
    Assert.Equal(uniqueName, _role.UniqueName);
    Assert.Contains(_role.Changes, change => change is RoleUniqueNameChangedEvent);

    _role.ClearChanges();
    _role.SetUniqueName(uniqueName);
    Assert.False(_role.HasChanges);
  }

  [Fact(DisplayName = "ToString: it should return the correct string representation.")]
  public void ToString_it_should_return_the_correct_string_representation()
  {
    Assert.StartsWith($"{_uniqueName.Value} | ", _role.ToString());

    DisplayNameUnit displayName = new("Administrator");
    _role.DisplayName = displayName;
    Assert.StartsWith($"{displayName.Value} | ", _role.ToString());
  }

  [Fact(DisplayName = "UniqueName: it should throw InvalidOperationException when it has not been initialized yet.")]
  public void UniqueName_it_should_throw_InvalidOperationException_when_it_has_not_been_initialized_yet()
  {
    RoleAggregate role = new(AggregateId.NewId());
    Assert.Throws<InvalidOperationException>(() => _ = role.UniqueName);
  }

  [Fact(DisplayName = "Update: it should update the role when it has changes.")]
  public void Update_it_should_update_the_role_when_it_has_changes()
  {
    ActorId actorId = ActorId.NewId();

    _role.DisplayName = new DisplayNameUnit("Administrator");
    _role.Update(actorId);
    Assert.Equal(actorId, _role.UpdatedBy);

    long version = _role.Version;
    _role.Update(actorId);
    Assert.Equal(version, _role.Version);
  }

  private static void AssertHasNoUpdate(RoleAggregate role)
  {
    FieldInfo? field = role.GetType().GetField("_updated", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(field);

    RoleUpdatedEvent? updated = field.GetValue(role) as RoleUpdatedEvent;
    Assert.NotNull(updated);
    Assert.False(updated.HasChanges);
  }
}
