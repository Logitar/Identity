using Logitar.Identity.Infrastructure.Converters;

namespace Logitar.Identity.Core.Users.Events;

[Trait(Traits.Category, Categories.Unit)]
public class UserUpdatedTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public UserUpdatedTests()
  {
    _serializerOptions.Converters.Add(new IdentifierConverter());
  }

  [Fact(DisplayName = "It should be serialized and deserialized correctly.")]
  public void Given_UpdatedEvent_When_Serialize_Then_SerializedCorrectly()
  {
    UserUpdated updated = new();
    updated.CustomAttributes[new Identifier("HealthInsuranceNumber")] = "1234567890";

    var json = JsonSerializer.Serialize(updated, _serializerOptions);
    var deserialized = JsonSerializer.Deserialize<UserUpdated>(json, _serializerOptions);

    Assert.NotNull(deserialized);
    Assert.Equal(updated.CustomAttributes, deserialized.CustomAttributes);
  }
}
