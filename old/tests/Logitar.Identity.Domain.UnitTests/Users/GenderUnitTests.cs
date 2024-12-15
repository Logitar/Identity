using Bogus;

namespace Logitar.Identity.Domain.Users;

[Trait(Traits.Category, Categories.Unit)]
public class GenderUnitTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should create a new gender.")]
  [InlineData("Unknown")]
  [InlineData(" Other ")]
  public void ctor_it_should_create_a_new_gender(string value)
  {
    GenderUnit gender = new(value);
    Assert.Equal(value.Trim(), gender.Value);
  }

  [Theory(DisplayName = "ctor: it should format a known gender.")]
  [InlineData(" Male ")]
  [InlineData("fEMALE")]
  public void ctor_it_should_format_a_known_gender(string value)
  {
    GenderUnit gender = new(value);
    Assert.Equal(value.Trim().ToLower(), gender.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(GenderUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new GenderUnit(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.Equal("NotEmptyValidator", e.ErrorCode);
    });
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the value is too long.")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_too_long()
  {
    string value = _faker.Random.String(GenderUnit.MaximumLength + 1, minChar: 'a', maxChar: 'z');
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new GenderUnit(value));
    Assert.All(exception.Errors, e => Assert.Equal("MaximumLengthValidator", e.ErrorCode));
  }

  [Theory(DisplayName = "IsKnown: it should return false when the value is not a known gender.")]
  [InlineData(" Other ")]
  [InlineData("Unknown")]
  public void IsKnown_it_should_return_false_when_the_value_is_not_a_known_gender(string value)
  {
    Assert.False(GenderUnit.IsKnown(value));
  }

  [Theory(DisplayName = "IsKnown: it should return true when the value is a known gender.")]
  [InlineData(" Male ")]
  [InlineData("fEMALE")]
  public void IsKnown_it_should_return_true_when_the_value_is_a_known_gender(string value)
  {
    Assert.True(GenderUnit.IsKnown(value));
  }

  [Theory(DisplayName = "TryCreate: it should return a gender when the value is not empty.")]
  [InlineData("Unknown")]
  [InlineData(" Other ")]
  public void TryCreate_it_should_return_a_gender_when_the_value_is_not_empty(string value)
  {
    GenderUnit? gender = GenderUnit.TryCreate(value);
    Assert.NotNull(gender);
    Assert.Equal(value.Trim(), gender.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(GenderUnit.TryCreate(value));
  }
}
