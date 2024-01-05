using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;

namespace Logitar.Identity.Infrastructure.Converters;

public class SessionIdConverter : JsonConverter<SessionId>
{
  public override SessionId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return value == null ? null : new(new AggregateId(value));
  }

  public override void Write(Utf8JsonWriter writer, SessionId sessionId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(sessionId.Value);
  }
}
