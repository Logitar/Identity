using Logitar.Identity.Core;
using Logitar.Identity.Core.Roles;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class RoleIdConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public RoleIdConverterTests()
  {
    _serializerOptions.Converters.Add(new RoleIdConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    RoleId roleId = RoleId.NewId(TenantId.NewId());
    RoleId deserialized = JsonSerializer.Deserialize<RoleId>(string.Concat('"', roleId, '"'), _serializerOptions);
    Assert.Equal(roleId, deserialized);
  }

  [Fact(DisplayName = "It should deserialize the default value from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_DefaultValue()
  {
    RoleId roleId = JsonSerializer.Deserialize<RoleId>("null", _serializerOptions);
    Assert.Equal(default, roleId);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    RoleId roleId = RoleId.NewId(TenantId.NewId());
    string json = JsonSerializer.Serialize(roleId, _serializerOptions);
    Assert.Equal(string.Concat('"', roleId, '"'), json);
  }
}
