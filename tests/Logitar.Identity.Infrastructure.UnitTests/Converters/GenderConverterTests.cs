using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class GenderConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public GenderConverterTests()
  {
    _serializerOptions.Converters.Add(new GenderConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    Gender gender = new("male");
    Gender? deserialized = JsonSerializer.Deserialize<Gender>(string.Concat('"', gender, '"'), _serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(gender, deserialized);
  }

  [Fact(DisplayName = "It should deserialize null from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_NullValue()
  {
    Gender? gender = JsonSerializer.Deserialize<Gender>("null", _serializerOptions);
    Assert.Null(gender);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    Gender gender = new("female");
    string json = JsonSerializer.Serialize(gender, _serializerOptions);
    Assert.Equal(string.Concat('"', gender, '"'), json);
  }
}
