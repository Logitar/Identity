using Bogus;

namespace Logitar.Identity.Domain.Users;

[Trait(Traits.Category, Categories.Unit)]
public class PersonNameUnitTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should create a new person name.")]
  [InlineData("PersonName")]
  [InlineData("  This is a person name.  ")]
  public void ctor_it_should_create_a_new_person_name(string value)
  {
    PersonNameUnit personName = new(value);
    Assert.Equal(value.Trim(), personName.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(PersonNameUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new PersonNameUnit(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.Equal("NotEmptyValidator", e.ErrorCode);
    });
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the value is too long.")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_too_long()
  {
    string value = _faker.Random.String(PersonNameUnit.MaximumLength + 1);
    string propertyName = nameof(PersonNameUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new PersonNameUnit(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal("MaximumLengthValidator", e.ErrorCode);
      Assert.Equal(propertyName, e.PropertyName);
    });
  }

  [Theory(DisplayName = "TryCreate: it should return a person name when the value is not empty.")]
  [InlineData("PersonName")]
  [InlineData("  This is a person name.  ")]
  public void TryCreate_it_should_return_a_person_name_when_the_value_is_not_empty(string value)
  {
    PersonNameUnit? personName = PersonNameUnit.TryCreate(value);
    Assert.NotNull(personName);
    Assert.Equal(value.Trim(), personName.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(PersonNameUnit.TryCreate(value));
  }
}
