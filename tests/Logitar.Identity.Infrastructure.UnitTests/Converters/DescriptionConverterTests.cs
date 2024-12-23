using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class DescriptionConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public DescriptionConverterTests()
  {
    _serializerOptions.Converters.Add(new DescriptionConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    Description description = new("Hello World!");
    Description? deserialized = JsonSerializer.Deserialize<Description>(string.Concat('"', description, '"'), _serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(description, deserialized);
  }

  [Fact(DisplayName = "It should deserialize null from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_NullValue()
  {
    Description? description = JsonSerializer.Deserialize<Description>("null", _serializerOptions);
    Assert.Null(description);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    Description description = new("Hello World!");
    string json = JsonSerializer.Serialize(description, _serializerOptions);
    Assert.Equal(string.Concat('"', description, '"'), json);
  }
}
