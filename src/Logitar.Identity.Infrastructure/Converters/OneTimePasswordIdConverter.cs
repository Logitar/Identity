using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Infrastructure.Converters;

public class OneTimePasswordIdConverter : JsonConverter<OneTimePasswordId>
{
  public override OneTimePasswordId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return OneTimePasswordId.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, OneTimePasswordId oneTimePasswordId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(oneTimePasswordId.Value);
  }
}
