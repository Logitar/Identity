using Bogus;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class UniqueNameConverterTests
{
  private readonly Faker _faker = new();
  private readonly JsonSerializerOptions _serializerOptions = new();
  private readonly UniqueNameSettings _uniqueNameSettings = new();

  public UniqueNameConverterTests()
  {
    _serializerOptions.Converters.Add(new UniqueNameConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    UniqueName uniqueName = new(_uniqueNameSettings, _faker.Person.UserName);
    UniqueName? deserialized = JsonSerializer.Deserialize<UniqueName>(string.Concat('"', uniqueName, '"'), _serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(uniqueName, deserialized);
  }

  [Fact(DisplayName = "It should deserialize null from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_NullValue()
  {
    UniqueName? uniqueName = JsonSerializer.Deserialize<UniqueName>("null", _serializerOptions);
    Assert.Null(uniqueName);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    UniqueName uniqueName = new(_uniqueNameSettings, _faker.Person.UserName);
    string json = JsonSerializer.Serialize(uniqueName, _serializerOptions);
    Assert.Equal(string.Concat('"', uniqueName, '"'), json);
  }
}
