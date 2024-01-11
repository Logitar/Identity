using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Infrastructure.Converters;

public class DescriptionConverter : JsonConverter<DescriptionUnit>
{
  public override DescriptionUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return value == null ? null : new(value); // TODO(fpion): shouldn't validate
  }

  public override void Write(Utf8JsonWriter writer, DescriptionUnit description, JsonSerializerOptions options)
  {
    writer.WriteStringValue(description.Value);
  }
}
