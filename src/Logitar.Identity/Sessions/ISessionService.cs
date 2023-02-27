namespace Logitar.Identity.Sessions;

/// <summary>
/// Exposes methods to manage user sessions in the identity system.
/// </summary>
public interface ISessionService
{
  /// <summary>
  /// Retrieves an user session by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the user session.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user session or null if not found.</returns>
  Task<Session?> GetAsync(Guid id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Retrieves a list of user sessions using the specified filters, sorting and paging arguments.
  /// </summary>
  /// <param name="isActive">The value filtering user sessions on their activation status.</param>
  /// <param name="isPersistent">The value filtering user sessions on their persistence status.</param>
  /// <param name="realm">The identifier or unique name of the realm to filter by.</param>
  /// <param name="userId">The identifier of the user to filter the sessions by.</param>
  /// <param name="sort">The sort value.</param>
  /// <param name="isDescending">If true, the sort will be inverted.</param>
  /// <param name="skip">The number of user sessions to skip.</param>
  /// <param name="take">The number of user sessions to return.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of user sessions, or empty if none found.</returns>
  Task<PagedList<Session>> GetAsync(bool? isActive = null, bool? isPersistent = null, string? realm = null, Guid? userId = null,
    SessionSort? sort = null, bool isDescending = false, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
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
