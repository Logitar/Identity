using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

public class LocaleConverter : JsonConverter<Locale>
{
  public override Locale? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return Locale.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, Locale locale, JsonSerializerOptions options)
  {
    writer.WriteStringValue(locale.Code);
  }
}
