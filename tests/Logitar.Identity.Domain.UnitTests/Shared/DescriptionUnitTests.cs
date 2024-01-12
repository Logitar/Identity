using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

[Trait(Traits.Category, Categories.Unit)]
public class DescriptionUnitTests
{
  [Theory(DisplayName = "ctor: it should create a new description.")]
  [InlineData("Description")]
  [InlineData("  This is a description.  ")]
  public void ctor_it_should_create_a_new_description(string value)
  {
    DescriptionUnit description = new(value);
    Assert.Equal(value.Trim(), description.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is not valid.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_not_valid(string value)
  {
    string propertyName = nameof(DescriptionUnit);

    var exception = Assert.Throws<ValidationException>(() => new DescriptionUnit(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.Equal("NotEmptyValidator", e.ErrorCode);
    });
  }

  [Theory(DisplayName = "TryCreate: it should return a description when the value is not empty.")]
  [InlineData("Description")]
  [InlineData("  This is a description.  ")]
  public void TryCreate_it_should_return_a_description_when_the_value_is_not_empty(string value)
  {
    DescriptionUnit? description = DescriptionUnit.TryCreate(value);
    Assert.NotNull(description);
    Assert.Equal(value.Trim(), description.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(DescriptionUnit.TryCreate(value));
  }
}
