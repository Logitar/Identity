using Logitar.EventSourcing;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Moq;

namespace Logitar.Identity.Domain.Roles;

[Trait(Traits.Category, Categories.Unit)]
public class RoleManagerTests
{
  private static readonly ActorId _actorId = default;
  private static readonly CancellationToken _cancellationToken = default;

  private readonly UniqueNameSettings _uniqueNameSettings = new();
  private readonly RoleAggregate _role;

  private readonly Mock<IApiKeyRepository> _apiKeyRepository = new();
  private readonly Mock<IRoleRepository> _roleRepository = new();
  private readonly Mock<IUserRepository> _userRepository = new();
  private readonly RoleManager _roleManager;

  public RoleManagerTests()
  {
    UniqueNameUnit uniqueName = new(_uniqueNameSettings, "admin");
    _role = new(uniqueName);

    _roleManager = new(_apiKeyRepository.Object, _roleRepository.Object, _userRepository.Object);
  }

  [Fact(DisplayName = "SaveAsync: it should not load any API key or user when it has not been deleted.")]
  public async Task SaveAsync_it_should_not_load_any_API_key_or_user_when_it_has_not_been_deleted()
  {
    _roleRepository.Setup(x => x.LoadAsync(_role.TenantId, _role.UniqueName, _cancellationToken)).ReturnsAsync(_role);

    await _roleManager.SaveAsync(_role, _actorId, _cancellationToken);

    _roleRepository.Verify(x => x.SaveAsync(_role, _cancellationToken), Times.Once);

    _apiKeyRepository.Verify(x => x.LoadAsync(_role, It.IsAny<CancellationToken>()), Times.Never);
    _userRepository.Verify(x => x.LoadAsync(_role, It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact(DisplayName = "SaveAsync: it should not load any role when the unique name has not changed.")]
  public async Task SaveAsync_it_should_not_load_any_role_when_the_unique_name_has_not_changed()
  {
    _role.ClearChanges();

    _role.DisplayName = new DisplayNameUnit("Administrator");
    _role.SetCustomAttribute("manage_users", bool.TrueString);
    _role.Update();

    await _roleManager.SaveAsync(_role, _actorId, _cancellationToken);

    _roleRepository.Verify(x => x.SaveAsync(_role, _cancellationToken), Times.Once);

    _roleRepository.Verify(x => x.LoadAsync(It.IsAny<TenantId>(), It.IsAny<UniqueNameUnit>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact(DisplayName = "SaveAsync: it should remove associations when it has been deleted.")]
  public async Task SaveAsync_it_should_remove_associations_when_it_has_been_deleted()
  {
    RoleAggregate guest = new(new UniqueNameUnit(_uniqueNameSettings, "guest"));

    DisplayNameUnit displayName = new("Test");
    PasswordMock secret = new("S3cr3+!*");
    ApiKeyAggregate apiKey = new(displayName, secret);
    apiKey.AddRole(_role);
    apiKey.AddRole(guest);
    _apiKeyRepository.Setup(x => x.LoadAsync(_role, _cancellationToken)).ReturnsAsync([apiKey]);

    UserAggregate user = new(new UniqueNameUnit(_uniqueNameSettings, "test"));
    user.AddRole(_role);
    user.AddRole(guest);
    _userRepository.Setup(x => x.LoadAsync(_role, _cancellationToken)).ReturnsAsync([user]);

    _role.Delete();
    await _roleManager.SaveAsync(_role, _actorId, _cancellationToken);

    _apiKeyRepository.Verify(x => x.SaveAsync(It.Is<IEnumerable<ApiKeyAggregate>>(y => y.Single().Equals(apiKey)), _cancellationToken), Times.Once);
    _roleRepository.Verify(x => x.SaveAsync(_role, _cancellationToken), Times.Once);
    _userRepository.Verify(x => x.SaveAsync(It.Is<IEnumerable<UserAggregate>>(y => y.Single().Equals(user)), _cancellationToken), Times.Once);

    Assert.Equal(guest.Id, apiKey.Roles.Single());
    Assert.Equal(guest.Id, user.Roles.Single());
  }

  [Fact(DisplayName = "SaveAsync: it should throw UniqueNameAlreadyUsedException when an unique name conflict occurs.")]
  public async Task SaveAsync_it_should_throw_UniqueNameAlreadyUsedException_when_an_unique_name_conflict_occurs()
  {
    RoleAggregate other = new(_role.UniqueName);
    _roleRepository.Setup(x => x.LoadAsync(_role.TenantId, _role.UniqueName, _cancellationToken)).ReturnsAsync(other);

    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<RoleAggregate>>(
      async () => await _roleManager.SaveAsync(_role, _actorId, _cancellationToken)
    );
    Assert.Equal(_role.TenantId, exception.TenantId);
    Assert.Equal(_role.UniqueName, exception.UniqueName);
  }
}
