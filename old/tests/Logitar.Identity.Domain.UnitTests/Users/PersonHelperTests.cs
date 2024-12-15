using Bogus;

namespace Logitar.Identity.Domain.Users;

[Trait(Traits.Category, Categories.Unit)]
public class PersonHelperTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "BuildFullNameString: it should return null when the list is empty.")]
  public void BuildFullNameString_it_should_return_null_when_the_list_is_empty()
  {
    Assert.Null(PersonHelper.BuildFullName(Array.Empty<string>()));
  }

  [Fact(DisplayName = "BuildFullNameString: it should return null when the list only contains empty names.")]
  public void BuildFullNameString_it_should_return_null_when_the_list_only_contains_empty_names()
  {
    Assert.Null(PersonHelper.BuildFullName(["", "   "]));
  }

  [Fact(DisplayName = "BuildFullNameString: it should build the full name of a person.")]
  public void BuildFullNameString_it_should_build_the_full_name_of_a_person()
  {
    string[] names = [_faker.Name.FirstName(), $"  {_faker.Name.FirstName()}  ", _faker.Name.LastName()];
    string expected = string.Join(' ', names.Select(name => name.Trim()));
    Assert.Equal(expected, PersonHelper.BuildFullName(names));
  }

  [Fact(DisplayName = "BuildFullNameUnit: it should return null when the list is empty.")]
  public void BuildFullNameUnit_it_should_return_null_when_the_list_is_empty()
  {
    Assert.Null(PersonHelper.BuildFullName(Array.Empty<PersonNameUnit>()));
  }

  [Fact(DisplayName = "BuildFullNameUnit: it should build the full name of a person.")]
  public void BuildFullNameUnit_it_should_build_the_full_name_of_a_person()
  {
    string[] names = [_faker.Name.FirstName(), $"  {_faker.Name.FirstName()}  ", _faker.Name.LastName()];
    string expected = string.Join(' ', names.Select(name => name.Trim()));
    Assert.Equal(expected, PersonHelper.BuildFullName(names.Select(name => new PersonNameUnit(name)).ToArray()));
  }
}
