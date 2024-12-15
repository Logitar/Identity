using Bogus;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Moq;

namespace Logitar.Identity.Domain.Users;

[Trait(Traits.Category, Categories.Unit)]
public class UserManagerTests
{
  private static readonly ActorId _actorId = default;
  private static readonly CancellationToken _cancellationToken = default;

  private readonly UserSettings _userSettings = new();
  private readonly UserAggregate _user;

  private readonly Faker _faker = new();
  private readonly Mock<ISessionRepository> _sessionRepository = new();
  private readonly Mock<IUserRepository> _userRepository = new();
  private readonly Mock<IUserSettingsResolver> _userSettingsResolver = new();
  private readonly UserManager _userManager;

  public UserManagerTests()
  {
    UniqueNameUnit uniqueName = new(_userSettings.UniqueName, "admin");
    _user = new(uniqueName);

    _userSettingsResolver.Setup(x => x.Resolve()).Returns(_userSettings);

    _userManager = new(_sessionRepository.Object, _userRepository.Object, _userSettingsResolver.Object);
  }

  [Fact(DisplayName = "FindAsync: it should find an user by email address.")]
  public async Task FindAsync_it_should_find_an_user_by_email_address()
  {
    _userSettings.RequireUniqueEmail = true;

    TenantId tenantId = new("tests");
    UserAggregate user = new(new UniqueNameUnit(_userSettings.UniqueName, _faker.Person.UserName));
    user.SetEmail(new EmailUnit(_faker.Person.Email));
    Assert.NotNull(user.Email);
    _userRepository.Setup(x => x.LoadAsync(tenantId, user.Email, _cancellationToken)).ReturnsAsync([user]);

    FoundUsers users = await _userManager.FindAsync(tenantId.Value, user.Email.Address, _cancellationToken);
    Assert.NotNull(users.ByEmail);
    Assert.Equal(user, users.ByEmail);
    Assert.Single(users.All);

    _userRepository.Verify(x => x.LoadAsync(It.IsAny<UserId>(), _cancellationToken), Times.Never);
    _userRepository.Verify(x => x.LoadAsync(tenantId, It.Is<UniqueNameUnit>(y => y.Value == user.Email.Address), _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.LoadAsync(tenantId, user.Email, _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "FindAsync: it should find an user by ID.")]
  public async Task FindAsync_it_should_find_an_user_by_Id()
  {
    UserAggregate user = new(new UniqueNameUnit(_userSettings.UniqueName, _faker.Person.UserName));
    _userRepository.Setup(x => x.LoadAsync(user.Id, _cancellationToken)).ReturnsAsync(user);

    FoundUsers users = await _userManager.FindAsync(tenantIdValue: null, user.Id.Value, _cancellationToken);
    Assert.NotNull(users.ById);
    Assert.Equal(user, users.ById);
    Assert.Single(users.All);

    _userRepository.Verify(x => x.LoadAsync(user.Id, _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.LoadAsync(null, It.Is<UniqueNameUnit>(y => y.Value == user.Id.Value), _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.LoadAsync(It.IsAny<TenantId>(), It.IsAny<EmailUnit>(), _cancellationToken), Times.Never);
  }

  [Fact(DisplayName = "FindAsync: it should find an user by unique name.")]
  public async Task FindAsync_it_should_find_an_user_by_unique_name()
  {
    TenantId tenantId = new("tests");
    UserAggregate user = new(new UniqueNameUnit(_userSettings.UniqueName, _faker.Person.UserName), tenantId);
    _userRepository.Setup(x => x.LoadAsync(tenantId, user.UniqueName, _cancellationToken)).ReturnsAsync(user);

    FoundUsers users = await _userManager.FindAsync(tenantId.Value, user.UniqueName.Value, _cancellationToken);
    Assert.NotNull(users.ByUniqueName);
    Assert.Equal(user, users.ByUniqueName);
    Assert.Single(users.All);

    _userRepository.Verify(x => x.LoadAsync(It.Is<UserId>(y => y.Value == user.UniqueName.Value), _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.LoadAsync(tenantId, user.UniqueName, _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.LoadAsync(It.IsAny<TenantId>(), It.IsAny<EmailUnit>(), _cancellationToken), Times.Never);
  }

  [Fact(DisplayName = "FindAsync: it should not find an user by email address when many are found.")]
  public async Task FindAsync_it_should_not_find_an_user_by_email_address_when_many_are_found()
  {
    _userSettings.RequireUniqueEmail = true;

    EmailUnit email = new(_faker.Internet.Email());
    UserAggregate user1 = new(new UniqueNameUnit(_userSettings.UniqueName, _faker.Internet.UserName()));
    user1.SetEmail(email);
    UserAggregate user2 = new(new UniqueNameUnit(_userSettings.UniqueName, _faker.Internet.UserName()));
    user2.SetEmail(email);
    _userRepository.Setup(x => x.LoadAsync(null, email, _cancellationToken)).ReturnsAsync([user1, user2]);

    FoundUsers users = await _userManager.FindAsync(tenantIdValue: null, email.Address, _cancellationToken);
    Assert.Empty(users.All);

    _userRepository.Verify(x => x.LoadAsync(It.IsAny<UserId>(), _cancellationToken), Times.Never);
    _userRepository.Verify(x => x.LoadAsync(null, It.Is<UniqueNameUnit>(y => y.Value == email.Address), _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.LoadAsync(null, email, _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "FindAsync: it should not search by email address when email addresses are not unique.")]
  public async Task FindAsync_it_should_not_search_by_email_address_when_email_addresses_are_not_unique()
  {
    string emailAddress = _faker.Person.Email;
    FoundUsers users = await _userManager.FindAsync(tenantIdValue: null, emailAddress, _cancellationToken);
    Assert.Empty(users.All);

    _userRepository.Verify(x => x.LoadAsync(It.IsAny<UserId>(), _cancellationToken), Times.Never);
    _userRepository.Verify(x => x.LoadAsync(null, It.Is<UniqueNameUnit>(y => y.Value == emailAddress), _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.LoadAsync(null, It.IsAny<EmailUnit>(), _cancellationToken), Times.Never);
  }

  [Fact(DisplayName = "FindAsync: it should not search by tenant id when it is not valid.")]
  public async Task FindAsync_it_should_not_search_by_tenant_id_when_it_is_not_valid()
  {
    string emailAddress = _faker.Person.Email;
    FoundUsers users = await _userManager.FindAsync(emailAddress, emailAddress, _cancellationToken);
    Assert.Empty(users.All);

    _userRepository.Verify(x => x.LoadAsync(It.IsAny<UserId>(), _cancellationToken), Times.Never);
    _userRepository.Verify(x => x.LoadAsync(null, It.Is<UniqueNameUnit>(y => y.Value == emailAddress), _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.LoadAsync(null, It.IsAny<EmailUnit>(), _cancellationToken), Times.Never);
  }

  [Fact(DisplayName = "SaveAsync: it should allow multiple email address when not unique.")]
  public async Task SaveAsync_it_should_allow_multiple_email_address_when_not_unique()
  {
    _user.SetEmail(new EmailUnit(_faker.Person.Email));

    await _userManager.SaveAsync(_user, _actorId, _cancellationToken);

    _userRepository.Verify(x => x.SaveAsync(_user, _cancellationToken), Times.Once);

    _userRepository.Verify(x => x.LoadAsync(It.IsAny<TenantId>(), It.IsAny<EmailUnit>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact(DisplayName = "SaveAsync: it should delete sessions when it has been deleted.")]
  public async Task SaveAsync_it_should_delete_sessions_when_it_has_been_deleted()
  {
    SessionAggregate session = new(_user);
    _sessionRepository.Setup(x => x.LoadAsync(_user, _cancellationToken)).ReturnsAsync([session]);

    _user.Delete();
    await _userManager.SaveAsync(_user, _actorId, _cancellationToken);

    _sessionRepository.Verify(x => x.SaveAsync(It.Is<IEnumerable<SessionAggregate>>(y => y.Single().Equals(session)), _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.SaveAsync(_user, _cancellationToken), Times.Once);

    Assert.True(session.IsDeleted);
  }

  [Fact(DisplayName = "SaveAsync: it should not load any session when it has not been deleted.")]
  public async Task SaveAsync_it_should_not_load_any_session_when_it_has_not_been_deleted()
  {
    _userRepository.Setup(x => x.LoadAsync(_user.TenantId, _user.UniqueName, _cancellationToken)).ReturnsAsync(_user);

    await _userManager.SaveAsync(_user, _actorId, _cancellationToken);

    _userRepository.Verify(x => x.SaveAsync(_user, _cancellationToken), Times.Once);

    _sessionRepository.Verify(x => x.LoadAsync(_user, It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact(DisplayName = "SaveAsync: it should not load any user when the unique name has not changed.")]
  public async Task SaveAsync_it_should_not_load_any_user_when_the_unique_name_has_not_changed()
  {
    _user.ClearChanges();

    _user.FirstName = new(_faker.Person.FirstName);
    _user.LastName = new(_faker.Person.LastName);
    _user.SetCustomAttribute("HealthInsuranceNumber", "1234567890");
    _user.Update();

    await _userManager.SaveAsync(_user, _actorId, _cancellationToken);

    _userRepository.Verify(x => x.SaveAsync(_user, _cancellationToken), Times.Once);

    _userRepository.Verify(x => x.LoadAsync(It.IsAny<TenantId>(), It.IsAny<UniqueNameUnit>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact(DisplayName = "SaveAsync: it should save the user when no custom identifier conflict occurs.")]
  public async Task SaveAsync_it_should_save_the_user_when_no_custom_identifier_conflict_occurs()
  {
    string identifierKey = "GoogleId";
    string identifierValue = Guid.NewGuid().ToString();

    _user.SetCustomIdentifier(identifierKey, identifierValue);
    _userRepository.Setup(x => x.LoadAsync(_user.TenantId, identifierKey, identifierValue, _cancellationToken)).ReturnsAsync(_user);

    await _userManager.SaveAsync(_user, _actorId, _cancellationToken);

    _userRepository.Verify(x => x.SaveAsync(_user, _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.LoadAsync(_user.TenantId, identifierKey, identifierValue, _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "SaveAsync: it should throw CustomIdentifierAlreadyUsedException when a custom identifier conflict occurs.")]
  public async Task SaveAsync_it_should_throw_CustomIdentifierAlreadyUsedException_when_a_custom_identifier_conflict_occurs()
  {
    string identifierKey = "GoogleId";
    string identifierValue = Guid.NewGuid().ToString();

    UserAggregate other = new(new UniqueNameUnit(_userSettings.UniqueName, "other"));
    other.SetCustomIdentifier(identifierKey, identifierValue);
    _userRepository.Setup(x => x.LoadAsync(_user.TenantId, identifierKey, identifierValue, _cancellationToken)).ReturnsAsync(other);

    _user.SetCustomIdentifier(identifierKey, identifierValue);

    var exception = await Assert.ThrowsAsync<CustomIdentifierAlreadyUsedException<UserAggregate>>(
      async () => await _userManager.SaveAsync(_user, _actorId, _cancellationToken)
    );
    Assert.Equal(_user.TenantId, exception.TenantId);
    Assert.Equal(identifierKey, exception.Key);
    Assert.Equal(identifierValue, exception.Value);
  }

  [Fact(DisplayName = "SaveAsync: it should throw EmailAddressAlreadyUsedException when an email address conflict occurs.")]
  public async Task SaveAsync_it_should_throw_EmailAddressAlreadyUsedException_when_an_email_address_conflict_occurs()
  {
    _userSettings.RequireUniqueEmail = true;

    EmailUnit email = new(_faker.Person.Email);

    UserAggregate other = new(new UniqueNameUnit(_userSettings.UniqueName, "other"));
    other.SetEmail(email);
    _userRepository.Setup(x => x.LoadAsync(_user.TenantId, email, _cancellationToken)).ReturnsAsync([other]);

    _user.SetEmail(email);

    var exception = await Assert.ThrowsAsync<EmailAddressAlreadyUsedException>(
      async () => await _userManager.SaveAsync(_user, _actorId, _cancellationToken)
    );
    Assert.Equal(_user.TenantId, exception.TenantId);
    Assert.Equal(email, exception.Email);
  }

  [Fact(DisplayName = "SaveAsync: it should throw UniqueNameAlreadyUsedException when an unique name conflict occurs.")]
  public async Task SaveAsync_it_should_throw_UniqueNameAlreadyUsedException_when_an_unique_name_conflict_occurs()
  {
    UserAggregate other = new(_user.UniqueName);
    _userRepository.Setup(x => x.LoadAsync(_user.TenantId, _user.UniqueName, _cancellationToken)).ReturnsAsync(other);

    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<UserAggregate>>(
      async () => await _userManager.SaveAsync(_user, _actorId, _cancellationToken)
    );
    Assert.Equal(_user.TenantId, exception.TenantId);
    Assert.Equal(_user.UniqueName, exception.UniqueName);
  }
}
