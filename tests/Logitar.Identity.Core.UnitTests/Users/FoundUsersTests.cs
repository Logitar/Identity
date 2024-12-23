using Bogus;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Core.Users;

[Trait(Traits.Category, Categories.Unit)]
public class FoundUsersTests
{
  private readonly Faker _faker = new();
  private readonly UniqueNameSettings _uniqueNameSettings = new();

  [Fact(DisplayName = "All: it should return all the users found.")]
  public void All_it_should_return_all_the_users_found()
  {
    FoundUsers users = new();
    Assert.Empty(users.All);

    User byId = new(new UniqueName(_uniqueNameSettings, _faker.Person.UserName));
    User byEmail = new(new UniqueName(_uniqueNameSettings, _faker.Person.Email));
    users = new()
    {
      ById = byId,
      ByEmail = byEmail
    };

    IEnumerable<User> all = users.All;
    Assert.Equal(2, all.Count());
    Assert.Contains(byId, all);
    Assert.Contains(byEmail, all);
  }

  [Fact(DisplayName = "Count: it should return the count of users found.")]
  public void Count_it_should_return_the_count_of_users_found()
  {
    FoundUsers users = new();
    Assert.Equal(0, users.Count);

    User byId = new(new UniqueName(_uniqueNameSettings, _faker.Person.UserName));
    User byEmail = new(new UniqueName(_uniqueNameSettings, _faker.Person.Email));
    users = new()
    {
      ById = byId,
      ByEmail = byEmail
    };
    Assert.Equal(2, users.Count);
  }

  [Fact(DisplayName = "First: it should return the first user found.")]
  public void First_it_should_return_the_first_user_found()
  {
    User byUniqueName = new(new UniqueName(_uniqueNameSettings, _faker.Person.UserName));
    User byEmail = new(new UniqueName(_uniqueNameSettings, _faker.Person.Email));
    FoundUsers users = new()
    {
      ByUniqueName = byUniqueName,
      ByEmail = byEmail
    };

    var first = users.First();
    Assert.Equal(byUniqueName, first);
  }

  [Fact(DisplayName = "FirstOrDefault: it should return the first user found or null if none found.")]
  public void FirstOrDefault_it_should_return_the_first_user_found_or_null_if_none_found()
  {
    FoundUsers users = new();
    Assert.Null(users.FirstOrDefault());

    User byUniqueName = new(new UniqueName(_uniqueNameSettings, _faker.Person.UserName));
    User byEmail = new(new UniqueName(_uniqueNameSettings, _faker.Person.Email));
    users = new()
    {
      ByUniqueName = byUniqueName,
      ByEmail = byEmail
    };

    var first = users.FirstOrDefault();
    Assert.NotNull(first);
    Assert.Equal(byUniqueName, first);
  }

  [Fact(DisplayName = "Single: it should return the only user found.")]
  public void Single_it_should_return_the_only_user_found()
  {
    User user = new(new UniqueName(_uniqueNameSettings, _faker.Person.UserName));
    FoundUsers users = new()
    {
      ById = user
    };

    var single = users.Single();
    Assert.Equal(user, single);
  }

  [Fact(DisplayName = "SingleOrDefault: it should return the only user found or null if none found.")]
  public void SingleOrDefault_it_should_return_the_only_user_found_or_null_if_none_found()
  {
    FoundUsers users = new();
    Assert.Null(users.SingleOrDefault());

    User byId = new(new UniqueName(_uniqueNameSettings, _faker.Person.UserName));
    users = new()
    {
      ById = byId
    };

    var single = users.SingleOrDefault();
    Assert.NotNull(single);
    Assert.Equal(byId, single);
  }
}
