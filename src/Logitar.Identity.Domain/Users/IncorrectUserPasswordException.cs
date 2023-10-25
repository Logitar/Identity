using FluentValidation.Results;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// The exception raised when an user password check fails.
/// </summary>
public class IncorrectUserPasswordException : InvalidCredentialsException, IValidationException
{
  private const string ErrorMessage = "The specified password did not match the user.";

  /// <summary>
  /// Gets or sets the attempted password.
  /// </summary>
  public string AttemptedPassword
  {
    get => (string)Data[nameof(AttemptedPassword)]!;
    private set => Data[nameof(AttemptedPassword)] = value;
  }
  /// <summary>
  /// Gets or sets the identifier of the user.
  /// </summary>
  public UserId UserId
  {
    get => new((string)Data[nameof(UserId)]!);
    private set => Data[nameof(UserId)] = value.Value;
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
  public ValidationFailure Failure => new(PropertyName, ErrorMessage, AttemptedPassword)
  {
    ErrorMessage = this.GetErrorCode(),
    CustomState = new
    {
      UserId = UserId.Value
    }
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="IncorrectUserPasswordException"/> class.
  /// </summary>
  /// <param name="attemptedPassword">The attempted password.</param>
  /// <param name="user">The user.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public IncorrectUserPasswordException(string attemptedPassword, UserAggregate user, string? propertyName = null)
    : base(BuildMessage(attemptedPassword, user, propertyName))
  {
    AttemptedPassword = attemptedPassword;
    UserId = user.Id;
    PropertyName = propertyName;
  }

  private static string BuildMessage(string attemptedPassword, UserAggregate user, string? propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(AttemptedPassword), attemptedPassword)
    .AddData(nameof(UserId), user.Id.Value)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
