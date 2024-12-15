using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Infrastructure.Converters;

public class PersonNameConverter : JsonConverter<PersonNameUnit>
{
  public override PersonNameUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return PersonNameUnit.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, PersonNameUnit personName, JsonSerializerOptions options)
  {
    writer.WriteStringValue(personName.Value);
  }
}
