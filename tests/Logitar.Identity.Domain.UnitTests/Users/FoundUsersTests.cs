using Bogus;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

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

    UserAggregate byId = new(new UniqueNameUnit(_uniqueNameSettings, _faker.Person.UserName));
    UserAggregate byEmail = new(new UniqueNameUnit(_uniqueNameSettings, _faker.Person.Email));
    users = new()
    {
      ById = byId,
      ByEmail = byEmail
    };

    IEnumerable<UserAggregate> all = users.All;
    Assert.Equal(2, all.Count());
    Assert.Contains(byId, all);
    Assert.Contains(byEmail, all);
  }

  [Fact(DisplayName = "First: it should return the first user found.")]
  public void First_it_should_return_the_first_user_found()
  {
    UserAggregate byUniqueName = new(new UniqueNameUnit(_uniqueNameSettings, _faker.Person.UserName));
    UserAggregate byEmail = new(new UniqueNameUnit(_uniqueNameSettings, _faker.Person.Email));
    FoundUsers users = new()
    {
      ByUniqueName = byUniqueName,
      ByEmail = byEmail
    };

    UserAggregate first = users.First();
    Assert.Equal(byUniqueName, first);
  }

  [Fact(DisplayName = "FirstOrDefault: it should return the first user found or null if none found.")]
  public void FirstOrDefault_it_should_return_the_first_user_found_or_null_if_none_found()
  {
    FoundUsers users = new();
    Assert.Null(users.FirstOrDefault());

    UserAggregate byUniqueName = new(new UniqueNameUnit(_uniqueNameSettings, _faker.Person.UserName));
    UserAggregate byEmail = new(new UniqueNameUnit(_uniqueNameSettings, _faker.Person.Email));
    users = new()
    {
      ByUniqueName = byUniqueName,
      ByEmail = byEmail
    };

    UserAggregate? first = users.FirstOrDefault();
    Assert.NotNull(first);
    Assert.Equal(byUniqueName, first);
  }

  [Fact(DisplayName = "Single: it should return the only user found.")]
  public void Single_it_should_return_the_only_user_found()
  {
    UserAggregate user = new(new UniqueNameUnit(_uniqueNameSettings, _faker.Person.UserName));
    FoundUsers users = new()
    {
      ById = user
    };

    UserAggregate single = users.Single();
    Assert.Equal(user, single);
  }

  [Fact(DisplayName = "SingleOrDefault: it should return the only user found or null if none found.")]
  public void SingleOrDefault_it_should_return_the_only_user_found_or_null_if_none_found()
  {
    FoundUsers users = new();
    Assert.Null(users.SingleOrDefault());

    UserAggregate byId = new(new UniqueNameUnit(_uniqueNameSettings, _faker.Person.UserName));
    users = new()
    {
      ById = byId
    };

    UserAggregate? single = users.SingleOrDefault();
    Assert.NotNull(single);
    Assert.Equal(byId, single);
  }
}
