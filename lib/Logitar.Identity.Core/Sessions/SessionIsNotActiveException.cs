namespace Logitar.Identity.Core.Sessions;

/// <summary>
/// The exception raised when an inactive session is renewed.
/// </summary>
public class SessionIsNotActiveException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified session is not active.";

  /// <summary>
  /// Gets the identifier of the inactive session.
  /// </summary>
  public string SessionId
  {
    get => (string)Data[nameof(SessionId)]!;
    private set => Data[nameof(SessionId)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionIsNotActiveException"/> class.
  /// </summary>
  /// <param name="session">The session that is not active.</param>
  public SessionIsNotActiveException(Session session) : base(BuildMessage(session))
  {
    SessionId = session.Id.Value;
  }

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="session">The session that is not active.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(Session session) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(SessionId), session.Id)
    .Build();
}
