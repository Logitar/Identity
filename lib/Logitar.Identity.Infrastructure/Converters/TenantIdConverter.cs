using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Converters;

public class TenantIdConverter : JsonConverter<TenantId>
{
  public override TenantId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return string.IsNullOrWhiteSpace(value) ? new TenantId() : new(value);
  }

  public override void Write(Utf8JsonWriter writer, TenantId tenantId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(tenantId.Value);
  }
}
