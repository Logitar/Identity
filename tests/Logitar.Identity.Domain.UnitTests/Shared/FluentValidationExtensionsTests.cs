namespace Logitar.Identity.Domain.Shared;

[Trait(Traits.Category, Categories.Unit)]
public class FluentValidationExtensionsTests
{
  [Fact(DisplayName = "BeAValidIdentifier: it should return false when it contains characters that are not allowed.")]
  public void BeAValidIdentifier_it_should_return_false_when_it_contains_characters_that_are_not_allowed()
  {
    Assert.False(FluentValidationExtensions.BeAValidIdentifier("-Test123!"));
  }

  [Fact(DisplayName = "BeAValidIdentifier: it should return false when it starts with a digit.")]
  public void BeAValidIdentifier_it_should_return_false_when_it_starts_with_a_digit()
  {
    Assert.False(FluentValidationExtensions.BeAValidIdentifier("123_tseT_"));
  }

  [Theory(DisplayName = "BeAValidIdentifier: it should return true when the value is a valid identifier.")]
  [InlineData("Test")]
  [InlineData("Test123")]
  [InlineData("_Test_123")]
  [InlineData("_123_")]
  public void BeAValidIdentifier_it_should_return_true_when_the_value_is_a_valid_identifier(string identifier)
  {
    Assert.True(FluentValidationExtensions.BeAValidIdentifier(identifier));
  }

  [Theory(DisplayName = "BeAValidUrl: it should return false when it is not a valid URL.")]
  [InlineData("")]
  [InlineData("    ")]
  [InlineData("test")]
  [InlineData("/about")]
  public void BeAValidUrl_it_should_return_false_when_it_is_not_a_valid_Url(string uriString)
  {
    Assert.False(FluentValidationExtensions.BeAValidUrl(uriString));
  }

  [Theory(DisplayName = "BeAValidUrl: it should return true when the value is a valid URL.")]
  [InlineData("http://test.com")]
  [InlineData("https://www.test.com/")]
  public void BeAValidUrl_it_should_return_true_when_the_value_is_a_valid_Url(string uriString)
  {
    Assert.True(FluentValidationExtensions.BeAValidUrl(uriString));
  }
}
