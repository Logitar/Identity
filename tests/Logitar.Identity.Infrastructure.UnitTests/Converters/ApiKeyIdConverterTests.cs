using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class ApiKeyIdConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public ApiKeyIdConverterTests()
  {
    _serializerOptions.Converters.Add(new ApiKeyIdConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    ApiKeyId apiKeyId = ApiKeyId.NewId(TenantId.NewId());
    ApiKeyId deserialized = JsonSerializer.Deserialize<ApiKeyId>(string.Concat('"', apiKeyId, '"'), _serializerOptions);
    Assert.Equal(apiKeyId, deserialized);
  }

  [Fact(DisplayName = "It should deserialize the default value from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_DefaultValue()
  {
    ApiKeyId apiKeyId = JsonSerializer.Deserialize<ApiKeyId>("null", _serializerOptions);
    Assert.Equal(default, apiKeyId);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    ApiKeyId apiKeyId = ApiKeyId.NewId(TenantId.NewId());
    string json = JsonSerializer.Serialize(apiKeyId, _serializerOptions);
    Assert.Equal(string.Concat('"', apiKeyId, '"'), json);
  }
}
