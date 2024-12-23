using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

public class EntityIdConverter : JsonConverter<EntityId>
{
  public override EntityId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return string.IsNullOrWhiteSpace(value) ? new EntityId() : new(value);
  }

  public override void Write(Utf8JsonWriter writer, EntityId entityId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(entityId.Value);
  }
}
