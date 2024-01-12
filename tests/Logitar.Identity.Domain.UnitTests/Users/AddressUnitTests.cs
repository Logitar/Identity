using Bogus;

namespace Logitar.Identity.Domain.Users;

[Trait(Traits.Category, Categories.Unit)]
public class AddressUnitTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should construct a new postal address.")]
  [InlineData("150 Saint-Catherine St W", "Montreal", "CA", "QC", "H2X 3Y2", false)]
  [InlineData(" 150 Saint-Catherine St W", " Montreal ", " CA ", " QC ", " H2X 3Y2 ", true)]
  public void ctor_it_should_construct_a_new_address_address(string street, string locality, string country, string? region, string? postalCode, bool isVerified)
  {
    AddressUnit address = new(street, locality, country, region, postalCode, isVerified);
    Assert.Equal(street.Trim(), address.Street);
    Assert.Equal(locality.Trim(), address.Locality);
    Assert.Equal(postalCode?.CleanTrim(), address.PostalCode);
    Assert.Equal(region?.CleanTrim(), address.Region);
    Assert.Equal(country.Trim(), address.Country);
    Assert.Equal(isVerified, address.IsVerified);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when a component is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_a_component_is_empty(string value)
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new AddressUnit(value, value, value, propertyName: "Address"));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Address.Street");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Address.Locality");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Address.Country");
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when a component is too long.")]
  public void ctor_it_should_throw_ValidationException_when_a_component_is_too_long()
  {
    string value = _faker.Random.String(AddressUnit.MaximumLength + 1, minChar: 'A', maxChar: 'Z');

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new AddressUnit(value, value, value, value, value, propertyName: "Address"));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "Address.Street");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "Address.Locality");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "Address.Country");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "Address.Region");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "Address.PostalCode");
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the country is not supported.")]
  public void ctor_it_should_throw_ValidationException_when_the_country_is_not_supported()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new AddressUnit("150 Saint-Catherine St W", "Montreal", "QC", propertyName: "Address"));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "CountryValidator" && e.PropertyName == "Address.Country");
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the postal code does not match the regular expression.")]
  public void ctor_it_should_throw_ValidationException_when_the_postal_code_does_not_match_the_regular_expression()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new AddressUnit("150 Saint-Catherine St W", "Montreal", "CA", "QC", "D0L7A9", propertyName: "Address"));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "PostalCodeValidator" && e.PropertyName == "Address.PostalCode");
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the region is not valid.")]
  public void ctor_it_should_throw_ValidationException_when_the_region_is_not_valid()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new AddressUnit("150 Saint-Catherine St W", "Montreal", "CA", "ZZ", "H2X 3Y2", propertyName: "Address"));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "RegionValidator" && e.PropertyName == "Address.Region");
  }

  [Fact(DisplayName = "Format: it should format a postal address.")]
  public void Format_it_should_format_a_postal_address()
  {
    AddressUnit address = new(" Jean Du Pays\r\n \r\n150 Saint-Catherine St W ", " Montreal ", " CA ", " QC ", " H2X 3Y2 ");
    string expected = string.Join(Environment.NewLine, ["Jean Du Pays", "150 Saint-Catherine St W", "Montreal QC H2X 3Y2", "CA"]);
    Assert.Equal(expected, address.Format());
  }
}
