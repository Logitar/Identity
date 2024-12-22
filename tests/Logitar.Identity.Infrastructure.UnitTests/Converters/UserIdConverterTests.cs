using Logitar.Identity.Core;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class UserIdConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public UserIdConverterTests()
  {
    _serializerOptions.Converters.Add(new UserIdConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    UserId userId = UserId.NewId(TenantId.NewId());
    UserId deserialized = JsonSerializer.Deserialize<UserId>(string.Concat('"', userId, '"'), _serializerOptions);
    Assert.Equal(userId, deserialized);
  }

  [Fact(DisplayName = "It should deserialize the default value from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_DefaultValue()
  {
    UserId userId = JsonSerializer.Deserialize<UserId>("null", _serializerOptions);
    Assert.Equal(default, userId);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    UserId userId = UserId.NewId(TenantId.NewId());
    string json = JsonSerializer.Serialize(userId, _serializerOptions);
    Assert.Equal(string.Concat('"', userId, '"'), json);
  }
}
