using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class IdentifierConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public IdentifierConverterTests()
  {
    _serializerOptions.Converters.Add(new IdentifierConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    Identifier identifier = new("HealthInsuranceNumber");
    Identifier? deserialized = JsonSerializer.Deserialize<Identifier>(string.Concat('"', identifier, '"'), _serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(identifier, deserialized);
  }

  [Fact(DisplayName = "It should deserialize null from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_NullValue()
  {
    Identifier? identifier = JsonSerializer.Deserialize<Identifier>("null", _serializerOptions);
    Assert.Null(identifier);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    Identifier identifier = new("HealthInsuranceNumber");
    string json = JsonSerializer.Serialize(identifier, _serializerOptions);
    Assert.Equal(string.Concat('"', identifier, '"'), json);
  }
}
