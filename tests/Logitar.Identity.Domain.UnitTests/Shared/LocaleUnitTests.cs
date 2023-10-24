using Bogus;

namespace Logitar.Identity.Domain.Shared;

[Trait(Traits.Category, Categories.Unit)]
public class LocaleUnitTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should create a new locale from a CultureInfo.")]
  [InlineData("en-CA")]
  [InlineData("en   ")]
  public void ctor_it_should_create_a_new_locale_from_a_CultureInfo(string value)
  {
    CultureInfo culture = CultureInfo.GetCultureInfo(value.Trim());
    LocaleUnit locale = new(culture);

    Assert.Equal(culture, locale.Culture);
    Assert.Equal(culture.Name, locale.Code);
  }

  [Theory(DisplayName = "ctor: it should create a new locale.")]
  [InlineData("fr-CA")]
  [InlineData("fr   ")]
  public void ctor_it_should_create_a_new_Locale_from_a_string(string value)
  {
    LocaleUnit locale = new(value);

    Assert.Equal(value.Trim(), locale.Code);
    Assert.Equal(CultureInfo.GetCultureInfo(value.Trim()), locale.Culture);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(LocaleUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new LocaleUnit(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.True(e.ErrorCode == "LocaleValidator" || e.ErrorCode == "NotEmptyValidator");
    });
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is not a valid locale code.")]
  [InlineData("")]
  [InlineData("  ")]
  [InlineData("en-BE")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_not_a_valid_locale_code(string value)
  {
    string propertyName = nameof(LocaleUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new LocaleUnit(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.True(e.ErrorCode == "LocaleValidator" || e.ErrorCode == "NotEmptyValidator");
    });
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is not a valid locale CultureInfo.")]
  [InlineData("")]
  [InlineData("  ")]
  [InlineData("fr-MX")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_not_a_valid_locale_CultureInfo(string value)
  {
    string propertyName = nameof(LocaleUnit);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new LocaleUnit(CultureInfo.GetCultureInfo(value.Trim()), propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.True(e.ErrorCode == "LocaleValidator" || e.ErrorCode == "NotEmptyValidator");
    });
  }

  [Theory(DisplayName = "TryCreate: it should return a locale when the value is not empty.")]
  [InlineData("es-MX")]
  [InlineData("   es")]
  public void TryCreate_it_should_return_a_locale_when_the_value_is_not_empty(string value)
  {
    LocaleUnit? locale = LocaleUnit.TryCreate(value);
    Assert.NotNull(locale);

    Assert.Equal(value.Trim(), locale.Code);
    Assert.Equal(CultureInfo.GetCultureInfo(value.Trim()), locale.Culture);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(LocaleUnit.TryCreate(value));
  }
}
