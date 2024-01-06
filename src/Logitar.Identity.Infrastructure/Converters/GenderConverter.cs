using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Infrastructure.Converters;

public class GenderConverter : JsonConverter<GenderUnit>
{
  public override GenderUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return value == null ? null : new(value); // TODO(fpion): shouldn't validate
  }

  public override void Write(Utf8JsonWriter writer, GenderUnit gender, JsonSerializerOptions options)
  {
    writer.WriteStringValue(gender.Value);
  }
}
