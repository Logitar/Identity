using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class EntityIdConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public EntityIdConverterTests()
  {
    _serializerOptions.Converters.Add(new EntityIdConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    EntityId entityId = EntityId.NewId();
    EntityId deserialized = JsonSerializer.Deserialize<EntityId>(string.Concat('"', entityId, '"'), _serializerOptions);
    Assert.Equal(entityId, deserialized);
  }

  [Fact(DisplayName = "It should deserialize the default value from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_DefaultValue()
  {
    EntityId entityId = JsonSerializer.Deserialize<EntityId>("null", _serializerOptions);
    Assert.Equal(default, entityId);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    EntityId entityId = EntityId.NewId();
    string json = JsonSerializer.Serialize(entityId, _serializerOptions);
    Assert.Equal(string.Concat('"', entityId, '"'), json);
  }
}
