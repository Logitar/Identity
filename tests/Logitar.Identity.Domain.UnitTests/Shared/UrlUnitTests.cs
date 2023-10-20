using Bogus;

namespace Logitar.Identity.Domain.Shared;

[Trait(Traits.Category, Categories.Unit)]
public class UrlUnitTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should create a new URL.")]
  [InlineData("http://test.com")]
  [InlineData("  https://www.test.com/  ")]
  public void ctor_it_should_create_a_new_Url_from_a_string(string value)
  {
    UrlUnit url = new(value);

    string expected = value.Trim();
    if (!expected.EndsWith('/'))
    {
      expected = string.Concat(expected, '/');
    }
    Assert.Equal(expected, url.Value);
    Assert.Equal(new Uri(value.Trim()), url.Uri);
  }

  [Theory(DisplayName = "ctor: it should create a new Url from an Uri.")]
  [InlineData("http://test.com")]
  [InlineData("  https://www.test.com/  ")]
  public void ctor_it_should_create_a_new_Url_from_an_Uri(string value)
  {
    Uri uri = new(value.Trim());
    UrlUnit url = new(uri);

    Assert.Equal(uri, url.Uri);
    Assert.Equal(uri.ToString(), url.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(UrlUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new UrlUnit(value, propertyName));
    Assert.All(exception.Errors, e => Assert.Equal(propertyName, e.PropertyName));
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the value is too long.")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_too_long()
  {
    string value = _faker.Random.String(UrlUnit.MaximumLength + 1);
    string propertyName = nameof(UrlUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new UrlUnit(value, propertyName));
    Assert.All(exception.Errors, e => Assert.Equal(propertyName, e.PropertyName));
  }

  [Theory(DisplayName = "TryCreate: it should return a URL when the value is not empty.")]
  [InlineData("http://test.com")]
  [InlineData("  https://www.test.com/  ")]
  public void TryCreate_it_should_return_a_Url_when_the_value_is_not_empty(string value)
  {
    UrlUnit? url = UrlUnit.TryCreate(value);
    Assert.NotNull(url);

    string expected = value.Trim();
    if (!expected.EndsWith('/'))
    {
      expected = string.Concat(expected, '/');
    }
    Assert.Equal(expected, url.Value);
    Assert.Equal(new Uri(value.Trim()), url.Uri);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(UrlUnit.TryCreate(value));
  }
}
