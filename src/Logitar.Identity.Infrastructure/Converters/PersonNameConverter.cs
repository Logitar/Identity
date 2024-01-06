﻿using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Infrastructure.Converters;

public class PersonNameConverter : JsonConverter<PersonNameUnit>
{
  public override PersonNameUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return value == null ? null : new(value); // TODO(fpion): shouldn't validate
  }

  public override void Write(Utf8JsonWriter writer, PersonNameUnit personName, JsonSerializerOptions options)
  {
    writer.WriteStringValue(personName.Value);
  }
}
