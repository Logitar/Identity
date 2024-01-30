using Bogus;

namespace Logitar.Identity.Domain.Sessions.Events;

[Trait(Traits.Category, Categories.Unit)]
public class SessionUpdatedEventTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "It should be serializable and deserializable.")]
  public void It_should_be_serializable_and_deserializable()
  {
    SessionUpdatedEvent @event = new();
    @event.CustomAttributes.Add("AdditionalInformation", $@"{{""User-Agent"":""{_faker.Internet.UserAgent()}""}}");
    @event.CustomAttributes.Add("IpAddress", _faker.Internet.Ip());

    string json = JsonSerializer.Serialize(@event);
    Assert.DoesNotContain("haschanges", json.ToLower());

    SessionUpdatedEvent? deserialized = JsonSerializer.Deserialize<SessionUpdatedEvent>(json);
    Assert.NotNull(deserialized);
    Assert.Equal(@event.CustomAttributes, deserialized.CustomAttributes);
  }
}
