using Logitar.Identity.Sessions.Commands;
using Logitar.Identity.Sessions.Queries;

namespace Logitar.Identity.Sessions;

/// <summary>
/// Implements methods to manage user sessions in the identity system.
/// </summary>
internal class SessionService : ISessionService
{
  /// <summary>
  /// The request pipeline.
  /// </summary>
  private readonly IRequestPipeline _requestPipeline;

  public SessionService(IRequestPipeline requestPipeline)
  {
    _requestPipeline = requestPipeline;
  }

  /// <summary>
  /// Retrieves an user session by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the user session.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user session or null if not found.</returns>
  public async Task<Session?> GetAsync(Guid id, CancellationToken cancellationToken)
  {
    return await _requestPipeline.ExecuteAsync(new GetSessionQuery(id), cancellationToken);
  }

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
  public async Task<PagedList<Session>> GetAsync(bool? isActive, bool? isPersistent, string? realm, Guid? userId,
    SessionSort? sort, bool isDescending, int? skip, int? take, CancellationToken cancellationToken)
  {
    return await _requestPipeline.ExecuteAsync(new GetSessionsQuery(isActive, isPersistent, realm, userId,
      sort, isDescending, skip, take), cancellationToken);
  }

  /// <summary>
  /// Refreshes the specified user sessions, generating a new refresh token.
  /// </summary>
  /// <param name="input">The session refresh input arguments.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The refreshed user session.</returns>
  public async Task<Session> RefreshAsync(RefreshSessionInput input, CancellationToken cancellationToken)
  {
    return await _requestPipeline.ExecuteAsync(new RefreshSessionCommand(input), cancellationToken);
  }

  /// <summary>
  /// Signs-in an user from the specified realm, opening a new user session.
  /// </summary>
  /// <param name="input">The sign-in input arguments.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The newly opened user session.</returns>
  public async Task<Session> SignInAsync(SignInInput input, CancellationToken cancellationToken)
  {
    return await _requestPipeline.ExecuteAsync(new SignInCommand(input), cancellationToken);
  }

  /// <summary>
  /// Signs-out the specified user session.
  /// </summary>
  /// <param name="id">The identifier of the user session.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The signed-out user session.</returns>
  public async Task<Session> SignOutAsync(Guid id, CancellationToken cancellationToken)
  {
    return await _requestPipeline.ExecuteAsync(new SignOutCommand(id), cancellationToken);
  }

  /// <summary>
  /// Signs-out the sessions of the specified user.
  /// </summary>
  /// <param name="id">The identifier of the user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The signed-out user sessions.</returns>
  public async Task<IEnumerable<Session>> SignOutUserAsync(Guid id, CancellationToken cancellationToken)
  {
    return await _requestPipeline.ExecuteAsync(new SignOutUserCommand(id), cancellationToken);
  }
}
