using Logitar.EventSourcing;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Infrastructure.Converters;

public class UserIdConverter : JsonConverter<UserId>
{
  public override UserId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    if (string.IsNullOrWhiteSpace(value))
    {
      return new UserId();
    }
    StreamId streamId = new(value);
    return new(streamId);
  }

  public override void Write(Utf8JsonWriter writer, UserId userId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(userId.Value);
  }
}
