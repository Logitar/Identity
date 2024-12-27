using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.Infrastructure.Passwords.Pbkdf2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeZone = Logitar.Identity.Core.TimeZone;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

[Trait(Traits.Category, Categories.Integration)]
public class UserPostgresIntegrationTests : IntegrationTests
{
  private const string PasswordString = "P@s$W0rD";

  private readonly IRoleRepository _roleRepository;
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;

  private readonly ActorId _actorId = ActorId.NewId();
  private readonly AddressHelper _helper = new();
  private readonly Password _password;
  private readonly UniqueNameSettings _uniqueNameSettings = new();

  public UserPostgresIntegrationTests() : base(DatabaseProvider.PostgreSQL)
  {
    _roleRepository = ServiceProvider.GetRequiredService<IRoleRepository>();
    _sessionRepository = ServiceProvider.GetRequiredService<ISessionRepository>();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();

    Pbkdf2Settings pbkdf2 = new();
    _password = new Pbkdf2Password(PasswordString, pbkdf2.Algorithm, pbkdf2.Iterations, pbkdf2.SaltLength, pbkdf2.HashLength);
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a deletion status.")]
  [InlineData(null)]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_Deleted_When_LoadAsync_Then_CorrectResult(bool? isDeleted)
  {
    User user1 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    User user2 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    user2.Delete();
    User user3 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    if (isDeleted == true)
    {
      user3.Delete();
    }
    if (isDeleted.HasValue)
    {
      await _userRepository.SaveAsync([user1, user2, user3]);
    }

    IReadOnlyCollection<User> users = await _userRepository.LoadAsync(isDeleted);

    if (isDeleted.HasValue)
    {
      Assert.Equal(2, users.Count);
      Assert.Contains(user3, users);
      Assert.Contains(isDeleted.Value ? user2 : user1, users);
    }
    else
    {
      Assert.Empty(users);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given IDs and a deletion status.")]
  [InlineData(null)]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_IdsDeleted_When_LoadAsync_Then_CorrectResult(bool? isDeleted)
  {
    User user1 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    User user2 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    user2.Delete();
    User user3 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    if (isDeleted == true)
    {
      user3.Delete();
    }
    User user4 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    if (isDeleted == true)
    {
      user4.Delete();
    }
    if (isDeleted.HasValue)
    {
      await _userRepository.SaveAsync([user1, user2, user3, user4]);
    }

    IReadOnlyCollection<User> users = await _userRepository.LoadAsync([user1.Id, user2.Id, user3.Id, UserId.NewId()], isDeleted);

    if (isDeleted.HasValue)
    {
      Assert.Equal(2, users.Count);
      Assert.Contains(user3, users);
      Assert.Contains(isDeleted.Value ? user2 : user1, users);
    }
    else
    {
      Assert.Empty(users);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given an ID, a version and a deletion status.")]
  [InlineData(false, null, false)]
  [InlineData(true, null, false)]
  [InlineData(true, false, false)]
  [InlineData(true, true, true)]
  public async Task Given_IdVersionDeleted_When_LoadAsync_Then_CorrectResult(bool found, bool? isDeleted, bool withVersion)
  {
    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    if (isDeleted == true)
    {
      user.Delete();
    }
    if (found)
    {
      await _userRepository.SaveAsync(user);
    }

    long? version = withVersion ? 1 : null;
    User? result = await _userRepository.LoadAsync(user.Id, version, isDeleted);
    if (found)
    {
      Assert.NotNull(result);
      if (withVersion)
      {
        Assert.Equal(version, result.Version);
        Assert.False(result.IsDeleted);
      }
      else
      {
        Assert.Equal(user.Version, result.Version);
        Assert.Equal(isDeleted ?? false, result.IsDeleted);
      }
    }
    else
    {
      Assert.Null(result);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a role.")]
  [InlineData(false)]
  [InlineData(true)]
  public async Task Given_Role_When_LoadAsync_Then_CorrectResults(bool found)
  {
    Role role = new(new UniqueName(new UniqueNameSettings(), "admin"));
    await _roleRepository.SaveAsync(role);

    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    if (found)
    {
      user.AddRole(role);
    }
    await _userRepository.SaveAsync(user);

    IReadOnlyCollection<User> result = await _userRepository.LoadAsync(role);

    if (found)
    {
      Assert.Equal(user, Assert.Single(result));
    }
    else
    {
      Assert.Empty(result);
    }
  }

  [Fact(DisplayName = "LoadAsync: it should return the correct result, given a session.")]
  public async Task Given_Session_When_LoadAsync_Then_CorrectResults()
  {
    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    await _userRepository.SaveAsync(user);

    Session session = user.SignIn();
    await _sessionRepository.SaveAsync(session);

    User? result = await _userRepository.LoadAsync(session);
    Assert.NotNull(result);
    Assert.Equal(user, result);
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a tenant ID.")]
  [InlineData(false, null)]
  [InlineData(true, null)]
  [InlineData(true, "3812eedf-7976-4784-92cd-2ae34ab4414d")]
  public async Task Given_TenantId_When_LoadAsync_Then_CorrectResults(bool found, string? tenantIdValue)
  {
    User user1 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName));
    TenantId tenantId = tenantIdValue == null ? TenantId.NewId() : new(tenantIdValue);
    User user2 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName), actorId: null, UserId.NewId(tenantId));
    if (found)
    {
      await _userRepository.SaveAsync([user1, user2]);
    }

    IReadOnlyCollection<User> users = await _userRepository.LoadAsync(tenantIdValue == null ? null : tenantId);

    if (found)
    {
      Assert.Equal(tenantIdValue == null ? user1 : user2, Assert.Single(users));
    }
    else
    {
      Assert.Empty(users);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a tenant ID and a custom identifier.")]
  [InlineData(false, null)]
  [InlineData(true, null)]
  [InlineData(true, "557ed780-ffbb-4d37-a3eb-89b2a451d560")]
  public async Task Given_TenantIdCustomIdentifier_When_LoadAsync_Then_CorrectResult(bool found, string? tenantIdValue)
  {
    Identifier identifierKey = new("HealthInsuranceNumber");
    CustomIdentifier identifierValue = new("1234567890");

    TenantId? tenantId = tenantIdValue == null ? null : new(tenantIdValue);
    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName), actorId: null, UserId.NewId(tenantId));
    if (found)
    {
      user.SetCustomIdentifier(identifierKey, identifierValue);
    }
    await _userRepository.SaveAsync(user);

    User? result = await _userRepository.LoadAsync(tenantId, identifierKey, identifierValue);

    if (found)
    {
      Assert.Equal(user, result);
    }
    else
    {
      Assert.Null(result);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a tenant ID and a unique name.")]
  [InlineData(false, null)]
  [InlineData(true, null)]
  [InlineData(true, "c256b969-ae4b-40a7-82c6-7da558f213be")]
  public async Task Given_TenantIdEmailAddress_When_LoadAsync_Then_CorrectResult(bool found, string? tenantIdValue)
  {
    TenantId? tenantId = tenantIdValue == null ? null : new(tenantIdValue);
    User user1 = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName), actorId: null, UserId.NewId(tenantId));
    User user2 = new(new UniqueName(_uniqueNameSettings, Faker.Internet.UserName()), actorId: null, UserId.NewId(tenantId));
    Email email = new(Faker.Person.Email);
    if (found)
    {
      user1.SetEmail(email);
      user2.SetEmail(email);
    }
    await _userRepository.SaveAsync([user1, user2]);

    IReadOnlyCollection<User> result = await _userRepository.LoadAsync(tenantId, email);

    if (found)
    {
      Assert.Equal(2, result.Count);
      Assert.Contains(user1, result);
      Assert.Contains(user2, result);
    }
    else
    {
      Assert.Empty(result);
    }
  }

  [Theory(DisplayName = "LoadAsync: it should return the correct result, given a tenant ID and an email address.")]
  [InlineData(null, null)]
  [InlineData(null, "admin")]
  [InlineData("6d342d8f-6ea7-4e62-9b65-967fa8a6237a", "superadmin")]
  public async Task Given_TenantIdUniqueName_When_LoadAsync_Then_CorrectResult(string? tenantIdValue, string? uniqueNameValue)
  {
    TenantId? tenantId = tenantIdValue == null ? null : new(tenantIdValue);
    UniqueName uniqueName = new(_uniqueNameSettings, uniqueNameValue ?? "default");
    User user = new(uniqueName, actorId: null, UserId.NewId(tenantId));
    await _userRepository.SaveAsync(user);

    User? result = await _userRepository.LoadAsync(tenantId, uniqueNameValue == null ? new UniqueName(_uniqueNameSettings, "null") : uniqueName);

    if (uniqueNameValue == null)
    {
      Assert.Null(result);
    }
    else
    {
      Assert.NotNull(result);
      Assert.Equal(user, result);
    }
  }

  [Fact(DisplayName = "SaveAsync: it should remove an API key role.")]
  public async Task Given_ApiKeyWithRole_When_SaveAsync_Then_RoleRemoved()
  {
    Role role = new(new UniqueName(new UniqueNameSettings(), "admin"));
    await _roleRepository.SaveAsync(role);

    User user = new(new UniqueName(new UniqueNameSettings(), Faker.Person.UserName));
    user.AddRole(role);

    await _userRepository.SaveAsync(user);

    UserEntity? entity = await IdentityContext.Users.AsNoTracking()
      .Include(x => x.Roles)
      .SingleOrDefaultAsync();
    Assert.NotNull(entity);
    Assert.Equal(user.Id.Value, entity.StreamId);
    Assert.Equal(role.Id.Value, Assert.Single(entity.Roles).StreamId);

    user.RemoveRole(role);
    await _userRepository.SaveAsync(user);

    entity = await IdentityContext.Users.AsNoTracking()
      .Include(x => x.Roles)
      .SingleOrDefaultAsync();
    Assert.NotNull(entity);
    Assert.Equal(user.Id.Value, entity.StreamId);
    Assert.Empty(entity.Roles);
  }

  [Fact(DisplayName = "SaveAsync: it should save the user correctly.")]
  public async Task Given_User_When_SaveAsync_Then_SavedCorrectly()
  {
    TenantId tenantId = TenantId.NewId();

    Role role = new(new UniqueName(new UniqueNameSettings(), "admin"), actorId: null, RoleId.NewId(tenantId));
    await _roleRepository.SaveAsync(role);

    User user = new(new UniqueName(_uniqueNameSettings, Faker.Person.UserName), _actorId, UserId.NewId(tenantId))
    {
      FirstName = new PersonName(Faker.Person.FirstName),
      MiddleName = new PersonName(Faker.Name.FirstName(Faker.Person.Gender)),
      LastName = new PersonName(Faker.Person.LastName),
      Nickname = new PersonName("goldy"),
      Birthdate = Faker.Person.DateOfBirth,
      Gender = new Gender(Faker.Person.Gender.ToString()),
      Locale = new Locale("fr-CA"),
      TimeZone = new TimeZone("America/Montreal"),
      Picture = new Url(Faker.Person.Avatar),
      Website = new Url($"https://www.{Faker.Person.Website}")
    };
    user.Profile = new Url($"https://www.{Faker.Internet.DomainName()}/profiles/{user.EntityId.ToGuid()}");
    user.SetCustomAttribute(new Identifier("UserType"), "Player");
    user.Update();
    user.AddRole(role);
    user.SetPassword(_password, _actorId);
    user.Authenticate(PasswordString);
    user.SetCustomIdentifier(new Identifier("HealthInsuranceNumber"), new CustomIdentifier("1234567890"));
    user.SetAddress(new Address(_helper, "150 Saint-Catherine St W", "Montreal", "CA", "QC", "H2X 3Y2"));
    user.SetEmail(new Email(Faker.Person.Email, isVerified: true));
    user.SetPhone(new Phone("514-845-4636", "CA", extension: "123546", isVerified: true));
    Session session = user.SignIn();
    user.SetUniqueName(new UniqueName(_uniqueNameSettings, Faker.Internet.UserName()));
    user.Disable(_actorId);
    await _userRepository.SaveAsync(user);
    await _sessionRepository.SaveAsync(session);

    UserEntity? entity = await IdentityContext.Users.AsNoTracking()
      .Include(x => x.Identifiers)
      .Include(x => x.Roles)
      .Include(x => x.Sessions)
      .SingleOrDefaultAsync();

    Assert.NotNull(entity);
    Assert.Equal(user.Id.Value, entity.StreamId);
    Assert.Equal(user.Version, entity.Version);
    Assert.Equal(user.CreatedBy?.Value, entity.CreatedBy);
    Assert.Equal(user.CreatedOn.AsUniversalTime(), entity.CreatedOn, TimeSpan.FromSeconds(5));
    Assert.Equal(user.UpdatedBy?.Value, entity.UpdatedBy);
    Assert.Equal(user.UpdatedOn.AsUniversalTime(), entity.UpdatedOn, TimeSpan.FromSeconds(5));
    Assert.Equal(tenantId.Value, entity.TenantId);
    Assert.Equal(user.EntityId.Value, entity.EntityId);
    Assert.Equal(user.UniqueName.Value, entity.UniqueName);
    Assert.Equal(user.UniqueName.Value.ToUpperInvariant(), entity.UniqueNameNormalized);
    Assert.Equal(_password.Encode(), entity.PasswordHash);
    Assert.Equal(_actorId.Value, entity.PasswordChangedBy);
    Assert.True(entity.PasswordChangedOn.HasValue);
    Assert.Equal(DateTime.UtcNow, entity.PasswordChangedOn.Value, TimeSpan.FromSeconds(5));
    Assert.True(entity.HasPassword);
    Assert.Equal(_actorId.Value, entity.DisabledBy);
    Assert.True(entity.DisabledOn.HasValue);
    Assert.Equal(DateTime.UtcNow, entity.DisabledOn.Value, TimeSpan.FromSeconds(5));
    Assert.True(entity.IsDisabled);
    Assert.NotNull(user.Address);
    Assert.Equal(user.Address.Street, entity.AddressStreet);
    Assert.Equal(user.Address.Locality, entity.AddressLocality);
    Assert.Equal(user.Address.PostalCode, entity.AddressPostalCode);
    Assert.Equal(user.Address.Region, entity.AddressRegion);
    Assert.Equal(user.Address.Country, entity.AddressCountry);
    Assert.Equal(user.Address.ToString(), entity.AddressFormatted);
    Assert.Null(entity.AddressVerifiedBy);
    Assert.Null(entity.AddressVerifiedOn);
    Assert.False(entity.IsAddressVerified);
    Assert.NotNull(user.Email);
    Assert.Equal(user.Email.Address, entity.EmailAddress);
    Assert.Equal(user.Email.Address.ToUpperInvariant(), entity.EmailAddressNormalized);
    Assert.Null(entity.EmailVerifiedBy);
    Assert.True(entity.EmailVerifiedOn.HasValue);
    Assert.Equal(DateTime.UtcNow, entity.EmailVerifiedOn.Value, TimeSpan.FromSeconds(5));
    Assert.True(entity.IsEmailVerified);
    Assert.NotNull(user.Phone);
    Assert.Equal(user.Phone.CountryCode, entity.PhoneCountryCode);
    Assert.Equal(user.Phone.Number, entity.PhoneNumber);
    Assert.Equal(user.Phone.Extension, entity.PhoneExtension);
    Assert.Equal(user.Phone.ToString(), entity.PhoneE164Formatted);
    Assert.Null(entity.PhoneVerifiedBy);
    Assert.True(entity.PhoneVerifiedOn.HasValue);
    Assert.Equal(DateTime.UtcNow, entity.PhoneVerifiedOn.Value, TimeSpan.FromSeconds(5));
    Assert.True(entity.IsPhoneVerified);
    Assert.True(entity.IsConfirmed);
    Assert.Equal(user.FirstName.Value, entity.FirstName);
    Assert.Equal(user.MiddleName.Value, entity.MiddleName);
    Assert.Equal(user.LastName.Value, entity.LastName);
    Assert.Equal(user.FullName, entity.FullName);
    Assert.Equal(user.Nickname.Value, entity.Nickname);
    Assert.True(entity.Birthdate.HasValue);
    Assert.Equal(user.Birthdate.Value.AsUniversalTime(), entity.Birthdate.Value, TimeSpan.FromSeconds(5));
    Assert.Equal(user.Gender.Value, entity.Gender);
    Assert.Equal(user.Locale.Code, entity.Locale);
    Assert.Equal(user.TimeZone.Id, entity.TimeZone);
    Assert.Equal(user.Picture.Value, entity.Picture);
    Assert.Equal(user.Profile.Value, entity.Profile);
    Assert.Equal(user.Website.Value, entity.Website);
    Assert.True(entity.AuthenticatedOn.HasValue);
    Assert.Equal(DateTime.UtcNow, entity.AuthenticatedOn.Value, TimeSpan.FromSeconds(5));
    Assert.Equal(@"{""UserType"":""Player""}", entity.CustomAttributes);
    Assert.Equal(role.Id.Value, Assert.Single(entity.Roles).StreamId);
    Assert.Equal(session.Id.Value, Assert.Single(entity.Sessions).StreamId);

    UserIdentifierEntity identifier = Assert.Single(entity.Identifiers);
    Assert.Equal(entity.TenantId, identifier.TenantId);
    Assert.Equal("HealthInsuranceNumber", identifier.Key);
    Assert.Equal("1234567890", identifier.Value);
    Assert.Equal(entity.UserId, identifier.UserId);

    ActorEntity? actor = await IdentityContext.Actors.AsNoTracking().SingleOrDefaultAsync();
    Assert.NotNull(actor);
    Assert.Equal(entity.StreamId, actor.Id);
    Assert.Equal(ActorType.User, actor.Type);
    Assert.False(actor.IsDeleted);
    Assert.Equal(entity.FullName, actor.DisplayName);
    Assert.Equal(entity.EmailAddress, actor.EmailAddress);
    Assert.Equal(entity.Picture, actor.PictureUrl);

    CustomAttributeEntity[] customAttributes = await IdentityContext.CustomAttributes.AsNoTracking().ToArrayAsync();
    Assert.Equal(user.CustomAttributes.Count, customAttributes.Length);
    foreach (KeyValuePair<Identifier, string> customAttribute in user.CustomAttributes)
    {
      Assert.Contains(customAttributes, c => c.EntityType == EntityType.User && c.EntityId == entity.UserId && c.Key == customAttribute.Key.Value
        && c.Value == customAttribute.Value && c.ValueShortened == customAttribute.Value.Truncate(byte.MaxValue));
    }

    session.Delete();
    await _sessionRepository.SaveAsync(session);

    user.Delete();
    await _userRepository.SaveAsync(user);

    entity = await IdentityContext.Users.AsNoTracking().SingleOrDefaultAsync();
    Assert.Null(entity);

    actor = await IdentityContext.Actors.AsNoTracking().SingleOrDefaultAsync();
    Assert.NotNull(actor);
    Assert.True(actor.IsDeleted);

    customAttributes = await IdentityContext.CustomAttributes.AsNoTracking().ToArrayAsync();
    Assert.Empty(customAttributes);
  }
}
