using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Defines methods to manage users.
/// </summary>
public interface IUserManager
{
  /// <summary>
  /// Tries finding an user by its unique identifier, unique name, or email address if they are unique.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which to search.</param>
  /// <param name="id">The identifier of the user to find.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<FoundUsers> FindAsync(string? tenantId, string id, CancellationToken cancellationToken = default);

  /// <summary>
  /// Saves the specified user, performing model validation such as unique name and email address unicity.
  /// </summary>
  /// <param name="user">The user to save.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(UserAggregate user, ActorId actorId = default, CancellationToken cancellationToken = default);
}
