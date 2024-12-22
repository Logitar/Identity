using Logitar.EventSourcing;
using Logitar.Identity.Core.Passwords;

namespace Logitar.Identity.Infrastructure.Converters;

public class OneTimePasswordIdConverter : JsonConverter<OneTimePasswordId>
{
  public override OneTimePasswordId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    if (string.IsNullOrWhiteSpace(value))
    {
      return new OneTimePasswordId();
    }
    StreamId streamId = new(value);
    return new(streamId);
  }

  public override void Write(Utf8JsonWriter writer, OneTimePasswordId oneTimePasswordId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(oneTimePasswordId.Value);
  }
}
