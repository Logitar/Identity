using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class CustomIdentifierConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public CustomIdentifierConverterTests()
  {
    _serializerOptions.Converters.Add(new CustomIdentifierConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    CustomIdentifier customIdentifier = new("1234567890");
    CustomIdentifier? deserialized = JsonSerializer.Deserialize<CustomIdentifier>(string.Concat('"', customIdentifier, '"'), _serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(customIdentifier, deserialized);
  }

  [Fact(DisplayName = "It should deserialize null from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_NullValue()
  {
    CustomIdentifier? customIdentifier = JsonSerializer.Deserialize<CustomIdentifier>("null", _serializerOptions);
    Assert.Null(customIdentifier);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    CustomIdentifier customIdentifier = new("1234567890");
    string json = JsonSerializer.Serialize(customIdentifier, _serializerOptions);
    Assert.Equal(string.Concat('"', customIdentifier, '"'), json);
  }
}
