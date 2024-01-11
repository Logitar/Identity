using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Sessions;

/// <summary>
/// The exception raised when a session secret check fails.
/// </summary>
public class IncorrectSessionSecretException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public new const string ErrorMessage = "The specified secret did not match the session.";

  /// <summary>
  /// Gets or sets the identifier of the session.
  /// </summary>
  public SessionId SessionId
  {
    get => new((string)Data[nameof(SessionId)]!);
    private set => Data[nameof(SessionId)] = value.Value;
  }
  /// <summary>
  /// Gets or sets the attempted secret.
  /// </summary>
  public string AttemptedSecret
  {
    get => (string)Data[nameof(AttemptedSecret)]!;
    private set => Data[nameof(AttemptedSecret)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="IncorrectSessionSecretException"/> class.
  /// </summary>
  /// <param name="attemptedSecret">The attempted secret.</param>
  /// <param name="session">The session.</param>
  public IncorrectSessionSecretException(SessionAggregate session, string attemptedSecret)
    : base(BuildMessage(session, attemptedSecret))
  {
    SessionId = session.Id;
    AttemptedSecret = attemptedSecret;
  }

  private static string BuildMessage(SessionAggregate session, string attemptedSecret) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(AttemptedSecret), attemptedSecret)
    .AddData(nameof(SessionId), session.Id.Value)
    .Build();
}
