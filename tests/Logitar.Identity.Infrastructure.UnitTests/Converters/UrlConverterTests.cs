using Bogus;
using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class UrlConverterTests
{
  private readonly Faker _faker = new();
  private readonly JsonSerializerOptions _serializerOptions = new();

  public UrlConverterTests()
  {
    _serializerOptions.Converters.Add(new UrlConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    Url url = new($"https://www.{_faker.Person.Website}");
    Url? deserialized = JsonSerializer.Deserialize<Url>(string.Concat('"', url, '"'), _serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(url, deserialized);
  }

  [Fact(DisplayName = "It should deserialize null from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_NullValue()
  {
    Url? url = JsonSerializer.Deserialize<Url>("null", _serializerOptions);
    Assert.Null(url);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    Url url = new($"https://www.{_faker.Person.Website}");
    string json = JsonSerializer.Serialize(url, _serializerOptions);
    Assert.Equal(string.Concat('"', url, '"'), json);
  }
}
