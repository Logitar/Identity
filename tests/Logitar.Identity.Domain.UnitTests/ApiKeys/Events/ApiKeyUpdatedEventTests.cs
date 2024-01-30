using Bogus;

namespace Logitar.Identity.Domain.ApiKeys.Events;

[Trait(Traits.Category, Categories.Unit)]
public class ApiKeyUpdatedEventTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "It should be serializable and deserializable.")]
  public void It_should_be_serializable_and_deserializable()
  {
    ApiKeyUpdatedEvent @event = new();
    @event.CustomAttributes.Add("Owner", _faker.Person.UserName);
    @event.CustomAttributes.Add("SubSystem", "Identity");

    string json = JsonSerializer.Serialize(@event);
    Assert.DoesNotContain("haschanges", json.ToLower());

    ApiKeyUpdatedEvent? deserialized = JsonSerializer.Deserialize<ApiKeyUpdatedEvent>(json);
    Assert.NotNull(deserialized);
    Assert.Equal(@event.CustomAttributes, deserialized.CustomAttributes);
  }
}
