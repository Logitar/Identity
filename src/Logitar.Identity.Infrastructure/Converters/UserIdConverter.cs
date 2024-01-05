using Logitar.EventSourcing;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Infrastructure.Converters;

public class UserIdConverter : JsonConverter<UserId>
{
  public override UserId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return value == null ? null : new(new AggregateId(value));
  }

  public override void Write(Utf8JsonWriter writer, UserId userId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(userId.Value);
  }
}
