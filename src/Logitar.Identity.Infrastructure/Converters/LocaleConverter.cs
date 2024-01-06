using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Infrastructure.Converters;

public class LocaleConverter : JsonConverter<LocaleUnit>
{
  public override LocaleUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? code = reader.GetString();
    return code == null ? null : new(code); // TODO(fpion): shouldn't validate
  }

  public override void Write(Utf8JsonWriter writer, LocaleUnit locale, JsonSerializerOptions options)
  {
    writer.WriteStringValue(locale.Code);
  }
}
