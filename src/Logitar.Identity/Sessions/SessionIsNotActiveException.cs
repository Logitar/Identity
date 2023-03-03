namespace Logitar.Identity.Sessions;

/// <summary>
/// The exception thrown when an user tries refreshing a signed-out session.
/// </summary>
public class SessionIsNotActiveException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionIsNotActiveException"/> class using the specified user session.
  /// </summary>
  /// <param name="session">The user that is not active.</param>
  public SessionIsNotActiveException(SessionAggregate session) : base($"The user session '{session}' is not active.")
  {
    Data["Session"] = session.ToString();
  }
}
