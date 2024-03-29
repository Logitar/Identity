﻿using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Infrastructure.Converters;

public class TenantIdConverter : JsonConverter<TenantId>
{
  public override TenantId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return TenantId.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, TenantId tenantId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(tenantId.Value);
  }
}
