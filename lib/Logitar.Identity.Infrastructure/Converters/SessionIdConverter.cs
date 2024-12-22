using Logitar.EventSourcing;
using Logitar.Identity.Core.Sessions;

namespace Logitar.Identity.Infrastructure.Converters;

public class SessionIdConverter : JsonConverter<SessionId>
{
  public override SessionId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    if (string.IsNullOrWhiteSpace(value))
    {
      return new SessionId();
    }
    StreamId streamId = new(value);
    return new(streamId);
  }

  public override void Write(Utf8JsonWriter writer, SessionId sessionId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(sessionId.Value);
  }
}
