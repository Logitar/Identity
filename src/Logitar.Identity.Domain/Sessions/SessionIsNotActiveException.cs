using FluentValidation.Results;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Sessions;

/// <summary>
/// The exception raised when an inactive session is signed-out.
/// </summary>
public class SessionIsNotActiveException : Exception, IFailureException
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
  /// Gets or sets the name of the validated property.
  /// </summary>
  public string? PropertyName
  {
    get => (string?)Data[nameof(PropertyName)];
    private set => Data[nameof(PropertyName)] = value;
  }

  /// <summary>
  /// Gets the validation failure of the exception.
  /// </summary>
  public ValidationFailure Failure => new(PropertyName, ErrorMessage, SessionId.Value)
  {
    ErrorCode = this.GetErrorCode()
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionIsNotActiveException"/> class.
  /// </summary>
  /// <param name="session">The session that is not active.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public SessionIsNotActiveException(SessionAggregate session, string? propertyName = null)
    : base(BuildMessage(session, propertyName))
  {
    SessionId = session.Id;
    PropertyName = propertyName;
  }

  private static string BuildMessage(SessionAggregate session, string? propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(SessionId), session.Id.Value)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
