using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Infrastructure.Converters;

public class UserIdConverter : JsonConverter<UserId>
{
  public override UserId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return UserId.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, UserId userId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(userId.Value);
  }
}
