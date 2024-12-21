using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles;

namespace Logitar.Identity.Infrastructure.Converters;

public class RoleIdConverter : JsonConverter<RoleId>
{
  public override RoleId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    if (string.IsNullOrWhiteSpace(value))
    {
      return new RoleId();
    }
    StreamId streamId = new(value);
    return new(streamId);
  }

  public override void Write(Utf8JsonWriter writer, RoleId roleId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(roleId.Value);
  }
}
