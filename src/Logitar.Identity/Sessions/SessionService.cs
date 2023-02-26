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
  /// Signs-in an user from the specified realm, opening a new user session.
  /// </summary>
  /// <param name="input">The sign-in input arguments.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The newly opened user session.</returns>
  public async Task<Session> SignInAsync(SignInInput input, CancellationToken cancellationToken)
  {
    return await _requestPipeline.ExecuteAsync(new SignInCommand(input), cancellationToken);
  }
}
