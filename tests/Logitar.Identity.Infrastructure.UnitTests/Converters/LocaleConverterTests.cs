using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class LocaleConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public LocaleConverterTests()
  {
    _serializerOptions.Converters.Add(new LocaleConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    Locale locale = new("en-US");
    Locale? deserialized = JsonSerializer.Deserialize<Locale>(string.Concat('"', locale.Code, '"'), _serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(locale, deserialized);
  }

  [Fact(DisplayName = "It should deserialize null from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_NullValue()
  {
    Locale? locale = JsonSerializer.Deserialize<Locale>("null", _serializerOptions);
    Assert.Null(locale);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    Locale locale = new("fr-CA");
    string json = JsonSerializer.Serialize(locale, _serializerOptions);
    Assert.Equal(string.Concat('"', locale.Code, '"'), json);
  }
}
