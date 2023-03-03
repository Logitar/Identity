using Logitar.Identity.Users;

namespace Logitar.Identity.Sessions;

/// <summary>
/// Exposes methods to load user sessions from the event store.
/// </summary>
public interface ISessionRepository
{
  /// <summary>
  /// Retrieves the list of the active sessions of the specified user.
  /// </summary>
  /// <param name="user">The user to retrieve its sessions.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of active sessions, or empty if none.</returns>
  Task<IEnumerable<SessionAggregate>> LoadActiveAsync(UserAggregate user, CancellationToken cancellationToken = default);
}
