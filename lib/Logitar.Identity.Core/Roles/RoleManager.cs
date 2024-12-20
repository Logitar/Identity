using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Roles.Events;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core.Roles;

/// <summary>
/// Implements methods to manage roles.
/// </summary>
public class RoleManager : IRoleManager
{
  /// <summary>
  /// Gets the API key repository.
  /// </summary>
  protected IApiKeyRepository ApiKeyRepository { get; }
  /// <summary>
  /// Gets the role repository.
  /// </summary>
  protected IRoleRepository RoleRepository { get; }
  /// <summary>
  /// Gets the user repository.
  /// </summary>
  protected IUserRepository UserRepository { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleManager"/> class.
  /// </summary>
  /// <param name="apiKeyRepository">The API key repository.</param>
  /// <param name="roleRepository">The role repository.</param>
  /// <param name="userRepository">The user repository.</param>
  public RoleManager(IApiKeyRepository apiKeyRepository, IRoleRepository roleRepository, IUserRepository userRepository)
  {
    ApiKeyRepository = apiKeyRepository;
    RoleRepository = roleRepository;
    UserRepository = userRepository;
  }

  /// <summary>
  /// Saves the specified user, performing model validation such as unique name unicity.
  /// </summary>
  /// <param name="role">The role to save.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public virtual async Task SaveAsync(Role role, ActorId? actorId, CancellationToken cancellationToken)
  {
    bool hasBeenDeleted = false;
    bool hasUniqueNameChanged = false;
    foreach (IEvent change in role.Changes)
    {
      if (change is RoleCreated || change is RoleUniqueNameChanged)
      {
        hasUniqueNameChanged = true;
      }
      else if (change is RoleDeleted)
      {
        hasBeenDeleted = true;
      }
    }

    if (hasUniqueNameChanged)
    {
      Role? conflict = await RoleRepository.LoadAsync(role.TenantId, role.UniqueName, cancellationToken);
      if (conflict != null && !conflict.Equals(role))
      {
        throw new UniqueNameAlreadyUsedException(role, conflict);
      }
    }

    if (hasBeenDeleted)
    {
      IEnumerable<ApiKey> apiKeys = await ApiKeyRepository.LoadAsync(role, cancellationToken);
      foreach (ApiKey apiKey in apiKeys)
      {
        apiKey.RemoveRole(role, actorId);
      }
      await ApiKeyRepository.SaveAsync(apiKeys, cancellationToken);

      IEnumerable<User> users = await UserRepository.LoadAsync(role, cancellationToken);
      foreach (User user in users)
      {
        user.RemoveRole(role, actorId);
      }
      await UserRepository.SaveAsync(users, cancellationToken);
    }

    await RoleRepository.SaveAsync(role, cancellationToken);
  }
}
