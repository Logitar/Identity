using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;

namespace Logitar.Identity.Domain.Users;

public class UserAggregate : AggregateRoot
{
  /// <summary>
  /// Signs-in the user without a password check, opening a new session.
  /// </summary>
  /// <param name="secret">The secret of the session.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <param name="sessionId">The identifier of the session.</param>
  /// <returns>The newly opened session.</returns>
  /// <exception cref="UserIsDisabledException">The user is disabled.</exception>
  public SessionAggregate SignIn(Password? secret = null, ActorId? actorId = null, SessionId? sessionId = null)
  {
    return SignIn(password: null, secret, actorId, sessionId);
  }
  /// <summary>
  /// Signs-in the user, opening a new session.
  /// </summary>
  /// <param name="password">The password to check.</param>
  /// <param name="secret">The secret of the session.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <param name="sessionId">The identifier of the session.</param>
  /// <returns>The newly opened session.</returns>
  /// <exception cref="IncorrectUserPasswordException">The password is incorrect.</exception>
  /// <exception cref="UserHasNoPasswordException">The user has no password.</exception>
  /// <exception cref="UserIsDisabledException">The user is disabled.</exception>
  public SessionAggregate SignIn(string? password, Password? secret = null, ActorId? actorId = null, SessionId? sessionId = null)
  {
    if (IsDisabled)
    {
      throw new UserIsDisabledException(this);
    }
    else if (password != null)
    {
      if (_password == null)
      {
        throw new UserHasNoPasswordException(this);
      }
      else if (!_password.IsMatch(password))
      {
        throw new IncorrectUserPasswordException(this, password);
      }
    }

    actorId ??= new(Id.Value);
    SessionAggregate session = new(this, secret, actorId, sessionId);
    Raise(new UserSignedInEvent(session.CreatedOn), actorId.Value);

    return session;
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserSignedInEvent @event)
  {
    AuthenticatedOn = @event.OccurredOn;
  }
}
