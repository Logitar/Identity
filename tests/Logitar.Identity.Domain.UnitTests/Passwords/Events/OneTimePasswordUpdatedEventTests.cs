namespace Logitar.Identity.Domain.Passwords.Events;

[Trait(Traits.Category, Categories.Unit)]
public class OneTimePasswordUpdatedEventTests
{
  [Fact(DisplayName = "It should be serializable and deserializable.")]
  public void It_should_be_serializable_and_deserializable()
  {
    OneTimePasswordUpdatedEvent @event = new();
    @event.CustomAttributes.Add("Purpose", "reset_password");
    @event.CustomAttributes.Add("UserId", Guid.NewGuid().ToString());

    string json = JsonSerializer.Serialize(@event);
    Assert.DoesNotContain("haschanges", json.ToLower());

    OneTimePasswordUpdatedEvent? deserialized = JsonSerializer.Deserialize<OneTimePasswordUpdatedEvent>(json);
    Assert.NotNull(deserialized);
    Assert.Equal(@event.CustomAttributes, deserialized.CustomAttributes);
  }
}
