using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Defines methods to manage users.
/// </summary>
public interface IUserManager
{
  /// <summary>
  /// Saves the specified user, performing model validation such as unique name and email address unicity.
  /// </summary>
  /// <param name="user">The user to save.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(UserAggregate user, ActorId actorId = default, CancellationToken cancellationToken = default);
}
