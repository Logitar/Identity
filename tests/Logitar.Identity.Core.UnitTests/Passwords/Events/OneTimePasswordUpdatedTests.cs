﻿using Logitar.Identity.Core.Users;
using Logitar.Identity.Infrastructure.Converters;

namespace Logitar.Identity.Core.Passwords.Events;

[Trait(Traits.Category, Categories.Unit)]
public class OneTimePasswordUpdatedTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public OneTimePasswordUpdatedTests()
  {
    _serializerOptions.Converters.Add(new IdentifierConverter());
  }

  [Fact(DisplayName = "It should be serialized and deserialized correctly.")]
  public void Given_UpdatedEvent_When_Serialize_Then_SerializedCorrectly()
  {
    OneTimePasswordUpdated updated = new();
    updated.CustomAttributes[new Identifier("UserId")] = UserId.NewId().Value;

    string json = JsonSerializer.Serialize(updated, _serializerOptions);
    OneTimePasswordUpdated? deserialized = JsonSerializer.Deserialize<OneTimePasswordUpdated>(json, _serializerOptions);

    Assert.NotNull(deserialized);
    Assert.Equal(updated.CustomAttributes, deserialized.CustomAttributes);
  }
}
