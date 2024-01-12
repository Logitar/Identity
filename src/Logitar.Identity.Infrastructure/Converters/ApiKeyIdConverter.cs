using Logitar.Identity.Domain.ApiKeys;

namespace Logitar.Identity.Infrastructure.Converters;

public class ApiKeyIdConverter : JsonConverter<ApiKeyId>
{
  public override ApiKeyId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return ApiKeyId.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, ApiKeyId apiKeyId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(apiKeyId.Value);
  }
}
