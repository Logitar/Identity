using Logitar.Identity.Core;
using Logitar.Identity.Core.Passwords;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class OneTimePasswordIdConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public OneTimePasswordIdConverterTests()
  {
    _serializerOptions.Converters.Add(new OneTimePasswordIdConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    OneTimePasswordId oneTimePasswordId = OneTimePasswordId.NewId(TenantId.NewId());
    OneTimePasswordId deserialized = JsonSerializer.Deserialize<OneTimePasswordId>(string.Concat('"', oneTimePasswordId, '"'), _serializerOptions);
    Assert.Equal(oneTimePasswordId, deserialized);
  }

  [Fact(DisplayName = "It should deserialize the default value from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_DefaultValue()
  {
    OneTimePasswordId oneTimePasswordId = JsonSerializer.Deserialize<OneTimePasswordId>("null", _serializerOptions);
    Assert.Equal(default, oneTimePasswordId);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    OneTimePasswordId oneTimePasswordId = OneTimePasswordId.NewId(TenantId.NewId());
    string json = JsonSerializer.Serialize(oneTimePasswordId, _serializerOptions);
    Assert.Equal(string.Concat('"', oneTimePasswordId, '"'), json);
  }
}
