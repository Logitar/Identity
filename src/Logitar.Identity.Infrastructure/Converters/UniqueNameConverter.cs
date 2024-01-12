using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Infrastructure.Converters;

public class UniqueNameConverter : JsonConverter<UniqueNameUnit>
{
  private readonly UniqueNameSettings _uniqueNameSettings = new()
  {
    AllowedCharacters = null // NOTE(fpion): strict validation is not required when deserializing an unique name.
  };

  public override UniqueNameUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return UniqueNameUnit.TryCreate(_uniqueNameSettings, reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, UniqueNameUnit uniqueName, JsonSerializerOptions options)
  {
    writer.WriteStringValue(uniqueName.Value);
  }
}
