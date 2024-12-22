using Bogus;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class PersonNameConverterTests
{
  private readonly Faker _faker = new();
  private readonly JsonSerializerOptions _serializerOptions = new();

  public PersonNameConverterTests()
  {
    _serializerOptions.Converters.Add(new PersonNameConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    PersonName personName = new(_faker.Person.FirstName);
    PersonName? deserialized = JsonSerializer.Deserialize<PersonName>(string.Concat('"', personName, '"'), _serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(personName, deserialized);
  }

  [Fact(DisplayName = "It should deserialize null from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_NullValue()
  {
    PersonName? personName = JsonSerializer.Deserialize<PersonName>("null", _serializerOptions);
    Assert.Null(personName);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    PersonName personName = new(_faker.Person.LastName);
    string json = JsonSerializer.Serialize(personName, _serializerOptions);
    Assert.Equal(string.Concat('"', personName, '"'), json);
  }
}
