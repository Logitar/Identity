using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class TenantIdConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public TenantIdConverterTests()
  {
    _serializerOptions.Converters.Add(new TenantIdConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    TenantId tenantId = TenantId.NewId();
    TenantId deserialized = JsonSerializer.Deserialize<TenantId>(string.Concat('"', tenantId, '"'), _serializerOptions);
    Assert.Equal(tenantId, deserialized);
  }

  [Fact(DisplayName = "It should deserialize the default value from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_DefaultValue()
  {
    TenantId tenantId = JsonSerializer.Deserialize<TenantId>("null", _serializerOptions);
    Assert.Equal(default, tenantId);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    TenantId tenantId = TenantId.NewId();
    string json = JsonSerializer.Serialize(tenantId, _serializerOptions);
    Assert.Equal(string.Concat('"', tenantId, '"'), json);
  }
}
