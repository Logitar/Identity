using Bogus;

namespace Logitar.Identity.Domain.Users.Events;

[Trait(Traits.Category, Categories.Unit)]
public class UserUpdatedEventTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "It should be serializable and deserializable.")]
  public void It_should_be_serializable_and_deserializable()
  {
    UserUpdatedEvent @event = new();
    @event.CustomAttributes.Add("HealthInsuranceNumber", _faker.Person.BuildHealthInsuranceNumber());
    @event.CustomAttributes.Add("JobTitle", "Sales Manager");

    string json = JsonSerializer.Serialize(@event);
    Assert.DoesNotContain("haschanges", json.ToLower());

    UserUpdatedEvent? deserialized = JsonSerializer.Deserialize<UserUpdatedEvent>(json);
    Assert.NotNull(deserialized);
    Assert.Equal(@event.CustomAttributes, deserialized.CustomAttributes);
  }
}
