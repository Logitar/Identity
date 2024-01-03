using Logitar.Identity.Domain.Shared;
using System.Text.Json;

namespace Logitar.Identity.Infrastructure.Converters;

public class TenantIdConverter : JsonConverter<TenantId>
{
  public override TenantId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return value == null ? null : new TenantId(value);
  }

  public override void Write(Utf8JsonWriter writer, TenantId value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }
}
