using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys;

namespace Logitar.Identity.Infrastructure.Converters;

public class ApiKeyIdConverter : JsonConverter<ApiKeyId>
{
  public override ApiKeyId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    if (string.IsNullOrWhiteSpace(value))
    {
      return new ApiKeyId();
    }
    StreamId streamId = new(value);
    return new(streamId);
  }

  public override void Write(Utf8JsonWriter writer, ApiKeyId apiKeyId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(apiKeyId.Value);
  }
}
