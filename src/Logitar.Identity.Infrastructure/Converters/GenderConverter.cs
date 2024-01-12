using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Infrastructure.Converters;

public class GenderConverter : JsonConverter<GenderUnit>
{
  public override GenderUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return GenderUnit.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, GenderUnit gender, JsonSerializerOptions options)
  {
    writer.WriteStringValue(gender.Value);
  }
}
