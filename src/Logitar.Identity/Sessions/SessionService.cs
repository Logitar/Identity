using Logitar.Identity.Sessions.Commands;

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
