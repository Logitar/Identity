namespace Logitar.Identity.Domain.Roles.Events;

[Trait(Traits.Category, Categories.Unit)]
public class RoleUpdatedEventTests
{
  [Fact(DisplayName = "It should be serializable and deserializable.")]
  public void It_should_be_serializable_and_deserializable()
  {
    RoleUpdatedEvent @event = new();
    @event.CustomAttributes.Add("manage_users", bool.FalseString);
    @event.CustomAttributes.Add("configuration", bool.TrueString);

    string json = JsonSerializer.Serialize(@event);
    Assert.DoesNotContain("haschanges", json.ToLower());

    RoleUpdatedEvent? deserialized = JsonSerializer.Deserialize<RoleUpdatedEvent>(json);
    Assert.NotNull(deserialized);
    Assert.Equal(@event.CustomAttributes, deserialized.CustomAttributes);
  }
}
