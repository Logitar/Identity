namespace Logitar.Identity.Domain.Sessions;

/// <summary>
/// The exception raised when an inactive session is signed-out.
/// </summary>
public class SessionIsNotActiveException : Exception
{
  private const string ErrorMessage = "The specified session is not active.";

  /// <summary>
  /// Gets the identifier of the inactive session.
  /// </summary>
  public SessionId SessionId
  {
    get => new((string)Data[nameof(SessionId)]!);
    private set => Data[nameof(SessionId)] = value.Value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionIsNotActiveException"/> class.
  /// </summary>
  /// <param name="session">The session that is not active.</param>
  public SessionIsNotActiveException(SessionAggregate session) : base(BuildMessage(session))
  {
    SessionId = session.Id;
  }

  private static string BuildMessage(SessionAggregate session) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(SessionId), session.Id.Value)
    .Build();
}
