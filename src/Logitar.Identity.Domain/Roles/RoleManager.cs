using Logitar.EventSourcing;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Roles;

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
  public virtual async Task SaveAsync(RoleAggregate role, ActorId actorId, CancellationToken cancellationToken)
  {
    bool hasBeenDeleted = false;
    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in role.Changes)
    {
      if (change is RoleCreatedEvent || change is RoleUniqueNameChangedEvent)
      {
        hasUniqueNameChanged = true;
      }
      else if (change is RoleDeletedEvent)
      {
        hasBeenDeleted = true;
      }
    }

    if (hasUniqueNameChanged)
    {
      RoleAggregate? other = await RoleRepository.LoadAsync(role.TenantId, role.UniqueName, cancellationToken);
      if (other?.Equals(role) == false)
      {
        throw new UniqueNameAlreadyUsedException<RoleAggregate>(role.TenantId, role.UniqueName);
      }
    }

    if (hasBeenDeleted)
    {
      IEnumerable<ApiKeyAggregate> apiKeys = await ApiKeyRepository.LoadAsync(role, cancellationToken);
      foreach (ApiKeyAggregate apiKey in apiKeys)
      {
        apiKey.RemoveRole(role, actorId);
      }
      await ApiKeyRepository.SaveAsync(apiKeys, cancellationToken);

      IEnumerable<UserAggregate> users = await UserRepository.LoadAsync(role, cancellationToken);
      foreach (UserAggregate user in users)
      {
        user.RemoveRole(role, actorId);
      }
      await UserRepository.SaveAsync(users, cancellationToken);
    }

    await RoleRepository.SaveAsync(role, cancellationToken);
  }
}
