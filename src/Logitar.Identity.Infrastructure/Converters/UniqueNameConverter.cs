using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Infrastructure.Converters;

public class UniqueNameConverter : JsonConverter<UniqueNameUnit>
{
  public override UniqueNameUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    UniqueNameSettings uniqueNameSettings = new()
    {
      AllowedCharacters = null
    };

    string? value = reader.GetString();
    return value == null ? null : new(uniqueNameSettings, value); // TODO(fpion): shouldn't validate
  }

  public override void Write(Utf8JsonWriter writer, UniqueNameUnit uniqueName, JsonSerializerOptions options)
  {
    writer.WriteStringValue(uniqueName.Value);
  }
}
