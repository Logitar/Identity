using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Infrastructure.Converters;

public class UrlConverter : JsonConverter<UrlUnit>
{
  public override UrlUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return UrlUnit.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, UrlUnit url, JsonSerializerOptions options)
  {
    writer.WriteStringValue(url.Value);
  }
}
