using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Infrastructure.Converters;

public class TimeZoneConverter : JsonConverter<TimeZoneUnit>
{
  public override TimeZoneUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return TimeZoneUnit.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, TimeZoneUnit timeZone, JsonSerializerOptions options)
  {
    writer.WriteStringValue(timeZone.Id);
  }
}
