using Logitar.Identity.Domain.ApiKeys;

namespace Logitar.Identity.Infrastructure.Converters;

public class ApiKeyIdConverter : JsonConverter<ApiKeyId>
{
  public override ApiKeyId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return value == null ? null : new(value);
  }

  public override void Write(Utf8JsonWriter writer, ApiKeyId apiKeyId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(apiKeyId.Value);
  }
}
