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
