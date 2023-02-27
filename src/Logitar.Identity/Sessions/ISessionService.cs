namespace Logitar.Identity.Sessions;

/// <summary>
/// Exposes methods to manage user sessions in the identity system.
/// </summary>
public interface ISessionService
{
  /// <summary>
  /// Refreshes the specified user sessions, generating a new refresh token.
  /// </summary>
  /// <param name="input">The session refresh input arguments.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The refreshed user session.</returns>
  Task<Session> RefreshAsync(RefreshSessionInput input, CancellationToken cancellationToken = default);
  /// <summary>
  /// Signs-in an user from the specified realm, opening a new user session.
  /// </summary>
  /// <param name="input">The sign-in input arguments.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The newly opened user session.</returns>
  Task<Session> SignInAsync(SignInInput input, CancellationToken cancellationToken = default);
  /// <summary>
  /// Signs-out the specified user session.
  /// </summary>
  /// <param name="id">The identifier of the user session.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The signed-out user session.</returns>
  Task<Session> SignOutAsync(Guid id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Signs-out the sessions of the specified user.
  /// </summary>
  /// <param name="id">The identifier of the user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The signed-out user sessions.</returns>
  Task<IEnumerable<Session>> SignOutUserAsync(Guid id, CancellationToken cancellationToken = default);
}
