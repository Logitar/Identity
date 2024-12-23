using Logitar.Identity.Core;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Infrastructure.Converters;

public class UniqueNameConverter : JsonConverter<UniqueName>
{
  private readonly UniqueNameSettings _uniqueNameSettings = new()
  {
    AllowedCharacters = null // NOTE(fpion): strict validation is not required when deserializing an unique name.
  };

  public override UniqueName? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return UniqueName.TryCreate(reader.GetString(), _uniqueNameSettings);
  }

  public override void Write(Utf8JsonWriter writer, UniqueName uniqueName, JsonSerializerOptions options)
  {
    writer.WriteStringValue(uniqueName.Value);
  }
}
