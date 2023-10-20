using Bogus;
using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Roles;

[Trait(Traits.Category, Categories.Unit)]
public class RoleIdTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should create a new role identifier.")]
  [InlineData("930a9c48-c168-47c8-9797-7106969dd7f7")]
  [InlineData("  admin  ")]
  public void ctor_it_should_create_a_new_display_name(string value)
  {
    RoleId roleId = new(value);
    Assert.Equal(value.Trim(), roleId.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(RoleId);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new RoleId(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.Equal("NotEmptyValidator", e.ErrorCode);
    });
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the value is too long.")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_too_long()
  {
    string value = _faker.Random.String(AggregateId.MaximumLength + 1);
    string propertyName = nameof(RoleId);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new RoleId(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal("MaximumLengthValidator", e.ErrorCode);
      Assert.Equal(propertyName, e.PropertyName);
    });
  }

  [Theory(DisplayName = "TryCreate: it should return a role identifier when the value is not empty.")]
  [InlineData("ede716c6-820a-4276-a3f9-d65645db7538")]
  [InlineData("  admin  ")]
  public void TryCreate_it_should_return_a_display_name_when_the_value_is_not_empty(string value)
  {
    RoleId? roleId = RoleId.TryCreate(value);
    Assert.NotNull(roleId);
    Assert.Equal(value.Trim(), roleId.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(RoleId.TryCreate(value));
  }
}
