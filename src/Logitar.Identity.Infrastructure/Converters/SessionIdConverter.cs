using Logitar.Identity.Domain.Sessions;

namespace Logitar.Identity.Infrastructure.Converters;

public class SessionIdConverter : JsonConverter<SessionId>
{
  public override SessionId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return SessionId.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, SessionId sessionId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(sessionId.Value);
  }
}
