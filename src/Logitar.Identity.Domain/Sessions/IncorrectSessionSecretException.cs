using FluentValidation.Results;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Sessions;

/// <summary>
/// The exception raised when a session secret check fails.
/// </summary>
public class IncorrectSessionSecretException : InvalidCredentialsException, IValidationException
{
  private const string ErrorMessage = "The specified secret did not match the session.";

  /// <summary>
  /// Gets or sets the attempted secret.
  /// </summary>
  public string AttemptedSecret
  {
    get => (string)Data[nameof(AttemptedSecret)]!;
    private set => Data[nameof(AttemptedSecret)] = value;
  }
  /// <summary>
  /// Gets or sets the identifier of the session.
  /// </summary>
  public SessionId SessionId
  {
    get => new((string)Data[nameof(SessionId)]!);
    private set => Data[nameof(SessionId)] = value.Value;
  }
  /// <summary>
  /// Gets or sets the name of the validated property.
  /// </summary>
  public string? PropertyName
  {
    get => (string)Data[nameof(PropertyName)]!;
    private set => Data[nameof(PropertyName)] = value;
  }

  /// <summary>
  /// Gets the validation failure of the exception.
  /// </summary>
  public ValidationFailure Failure => new(PropertyName, ErrorMessage, AttemptedSecret)
  {
    ErrorMessage = this.GetErrorCode(),
    CustomState = new
    {
      SessionId = SessionId.Value
    }
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="IncorrectSessionSecretException"/> class.
  /// </summary>
  /// <param name="attemptedSecret">The attempted secret.</param>
  /// <param name="session">The session.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public IncorrectSessionSecretException(string attemptedSecret, SessionAggregate session, string? propertyName = null)
    : base(BuildMessage(attemptedSecret, session, propertyName))
  {
    AttemptedSecret = attemptedSecret;
    SessionId = session.Id;
    PropertyName = propertyName;
  }

  private static string BuildMessage(string attemptedSecret, SessionAggregate session, string? propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(AttemptedSecret), attemptedSecret)
    .AddData(nameof(SessionId), session.Id.Value)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
