using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Infrastructure.Converters;

public class RoleIdConverter : JsonConverter<RoleId>
{
  public override RoleId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return RoleId.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, RoleId roleId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(roleId.Value);
  }
}
