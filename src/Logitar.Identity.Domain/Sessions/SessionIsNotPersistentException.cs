using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Sessions;

/// <summary>
/// The exception raised when an ephemeral (not persistent) session is renewed.
/// </summary>
public class SessionIsNotPersistentException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public new const string ErrorMessage = "The specified session is not persistent.";

  /// <summary>
  /// Gets the identifier of the ephemeral session.
  /// </summary>
  public SessionId SessionId
  {
    get => new((string)Data[nameof(SessionId)]!);
    private set => Data[nameof(SessionId)] = value.Value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionIsNotPersistentException"/> class.
  /// </summary>
  /// <param name="session">The session that is ephemeral.</param>
  public SessionIsNotPersistentException(SessionAggregate session) : base(BuildMessage(session))
  {
    SessionId = session.Id;
  }

  private static string BuildMessage(SessionAggregate session) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(SessionId), session.Id.Value)
    .Build();
}
