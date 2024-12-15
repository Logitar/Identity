using Bogus;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Domain.Shared;

[Trait(Traits.Category, Categories.Unit)]
public class UniqueNameUnitTests
{
  private readonly Faker _faker = new();
  private readonly UniqueNameSettings _uniqueNameSettings = new();

  [Theory(DisplayName = "ctor: it should create a new unique name.")]
  [InlineData("admin")]
  [InlineData("  admin@test.com  ")]
  public void ctor_it_should_create_a_new_unique_name(string value)
  {
    UniqueNameUnit uniqueName = new(_uniqueNameSettings, value);
    Assert.Equal(value.Trim(), uniqueName.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value contains characters that are not allowed.")]
  [InlineData(" test user ")]
  [InlineData("test:user")]
  public void ctor_it_should_throw_ValidationException_when_the_value_contains_characters_that_are_not_allowed(string value)
  {
    string propertyName = nameof(UniqueNameUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new UniqueNameUnit(_uniqueNameSettings, value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.Equal("AllowedCharactersValidator", e.ErrorCode);
    });
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(UniqueNameUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new UniqueNameUnit(_uniqueNameSettings, value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.Equal("NotEmptyValidator", e.ErrorCode);
    });
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the value is too long.")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_too_long()
  {
    string value = _faker.Random.String(UniqueNameUnit.MaximumLength + 1, minChar: 'a', maxChar: 'z');
    string propertyName = nameof(UniqueNameUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new UniqueNameUnit(_uniqueNameSettings, value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal("MaximumLengthValidator", e.ErrorCode);
      Assert.Equal(propertyName, e.PropertyName);
    });
  }

  [Theory(DisplayName = "TryCreate: it should return a unique name when the value is not empty.")]
  [InlineData("admin")]
  [InlineData("  admin@test.com  ")]
  public void TryCreate_it_should_return_a_unique_name_when_the_value_is_not_empty(string value)
  {
    UniqueNameUnit? uniqueName = UniqueNameUnit.TryCreate(_uniqueNameSettings, value);
    Assert.NotNull(uniqueName);
    Assert.Equal(value.Trim(), uniqueName.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(UniqueNameUnit.TryCreate(_uniqueNameSettings, value));
  }
}
