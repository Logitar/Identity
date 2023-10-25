using FluentValidation.Results;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// The exception raised when a disabled user is authenticated or signs-in.
/// </summary>
public class UserIsDisabledException : Exception, IFailureException
{
  private const string ErrorMessage = "The specified user is disabled.";

  /// <summary>
  /// Gets the identifier of the disabled user.
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
    get => (string?)Data[nameof(PropertyName)];
    private set => Data[nameof(PropertyName)] = value;
  }

  /// <summary>
  /// Gets the validation failure of the exception.
  /// </summary>
  public ValidationFailure Failure => new(PropertyName, ErrorMessage, UserId.Value)
  {
    ErrorCode = this.GetErrorCode()
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="UserIsDisabledException"/> class.
  /// </summary>
  /// <param name="user">The user that is disabled.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public UserIsDisabledException(UserAggregate user, string? propertyName = null)
    : base(BuildMessage(user, propertyName))
  {
    UserId = user.Id;
    PropertyName = propertyName;
  }

  private static string BuildMessage(UserAggregate user, string? propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(UserId), user.Id.Value)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
