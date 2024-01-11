using Bogus;

namespace Logitar.Identity.Domain.Shared;

[Trait(Traits.Category, Categories.Unit)]
public class DisplayNameUnitTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should create a new display name.")]
  [InlineData("DisplayName")]
  [InlineData("  This is a display name.  ")]
  public void ctor_it_should_create_a_new_display_name(string value)
  {
    DisplayNameUnit displayName = new(value);
    Assert.Equal(value.Trim(), displayName.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(DisplayNameUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new DisplayNameUnit(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.Equal("NotEmptyValidator", e.ErrorCode);
    });
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the value is too long.")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_too_long()
  {
    string value = _faker.Random.String(DisplayNameUnit.MaximumLength + 1);
    string propertyName = nameof(DisplayNameUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new DisplayNameUnit(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal("MaximumLengthValidator", e.ErrorCode);
      Assert.Equal(propertyName, e.PropertyName);
    });
  }

  [Theory(DisplayName = "TryCreate: it should return a display name when the value is not empty.")]
  [InlineData("DisplayName")]
  [InlineData("  This is a display name.  ")]
  public void TryCreate_it_should_return_a_display_name_when_the_value_is_not_empty(string value)
  {
    DisplayNameUnit? displayName = DisplayNameUnit.TryCreate(value);
    Assert.NotNull(displayName);
    Assert.Equal(value.Trim(), displayName.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(DisplayNameUnit.TryCreate(value));
  }
}
