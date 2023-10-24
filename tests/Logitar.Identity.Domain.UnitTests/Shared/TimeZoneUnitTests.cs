using Bogus;
using NodaTime;

namespace Logitar.Identity.Domain.Shared;

[Trait(Traits.Category, Categories.Unit)]
public class TimeZoneUnitTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should create a new time zone from a DateTimeZone.")]
  [InlineData("America/New_York")]
  [InlineData("  America/Montreal  ")]
  public void ctor_it_should_create_a_new_time_zone_from_a_DateTimeZone(string value)
  {
    DateTimeZone? tz = DateTimeZoneProviders.Tzdb.GetZoneOrNull(value.Trim());
    Assert.NotNull(tz);
    TimeZoneUnit timeZone = new(tz);

    Assert.Equal(tz, timeZone.TimeZone);
    Assert.Equal(tz.Id, timeZone.Id);
  }

  [Theory(DisplayName = "ctor: it should create a new time zone.")]
  [InlineData("America/New_York")]
  [InlineData("  America/Montreal  ")]
  public void ctor_it_should_create_a_new_time_zone_from_a_string(string value)
  {
    TimeZoneUnit timeZone = new(value);

    Assert.Equal(value.Trim(), timeZone.Id);
    Assert.Equal(DateTimeZoneProviders.Tzdb.GetZoneOrNull(value.Trim())?.Id, timeZone.Id);
  }

  [Theory(DisplayName = "ctor: it should throw InvalidTimeZoneEntryException when the value is not a valid tz identifier.")]
  [InlineData("")]
  [InlineData("    ")]
  [InlineData("America/Québec")]
  public void ctor_it_should_throw_InvalidTimeZoneEntryException_when_the_value_is_not_a_valid_tz_identifier(string value)
  {
    string propertyName = nameof(TimeZoneUnit);

    var exception = Assert.Throws<InvalidTimeZoneEntryException>(() => new TimeZoneUnit(value, propertyName));
    Assert.Equal(value, exception.Id);
    Assert.Equal(propertyName, exception.PropertyName);
  }

  [Theory(DisplayName = "TryCreate: it should return a time zone when the value is not empty.")]
  [InlineData("America/New_York")]
  [InlineData("  America/Montreal  ")]
  public void TryCreate_it_should_return_a_time_zone_when_the_value_is_not_empty(string value)
  {
    TimeZoneUnit? timeZone = TimeZoneUnit.TryCreate(value);
    Assert.NotNull(timeZone);

    Assert.Equal(value.Trim(), timeZone.Id);
    Assert.Equal(DateTimeZoneProviders.Tzdb.GetZoneOrNull(value.Trim()), timeZone.TimeZone);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(TimeZoneUnit.TryCreate(value));
  }
}
