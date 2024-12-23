using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class DisplayNameConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public DisplayNameConverterTests()
  {
    _serializerOptions.Converters.Add(new DisplayNameConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    DisplayName displayName = new("Administrator");
    DisplayName? deserialized = JsonSerializer.Deserialize<DisplayName>(string.Concat('"', displayName, '"'), _serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(displayName, deserialized);
  }

  [Fact(DisplayName = "It should deserialize null from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_NullValue()
  {
    DisplayName? displayName = JsonSerializer.Deserialize<DisplayName>("null", _serializerOptions);
    Assert.Null(displayName);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    DisplayName displayName = new("Administrator");
    string json = JsonSerializer.Serialize(displayName, _serializerOptions);
    Assert.Equal(string.Concat('"', displayName, '"'), json);
  }
}
