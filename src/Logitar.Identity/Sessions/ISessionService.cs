using Logitar.Identity.Accounts;

namespace Logitar.Identity.Sessions;

/// <summary>
/// Exposes methods to manage user sessions in the identity system.
/// </summary>
public interface ISessionService
{
  /// <summary>
  /// Signs-in an user from the specified realm, opening a new user session.
  /// </summary>
  /// <param name="input">The sign-in input arguments.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The newly opened user session.</returns>
  Task<Session> SignInAsync(SignInInput input, CancellationToken cancellationToken = default);
}
