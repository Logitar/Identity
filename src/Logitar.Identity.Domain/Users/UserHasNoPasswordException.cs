using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// The exception raised when handling an user without a password.
/// </summary>
public class UserHasNoPasswordException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public new const string ErrorMessage = "The specified user has no password.";

  /// <summary>
  /// Gets or sets the identifier of the user.
  /// </summary>
  public UserId UserId
  {
    get => new((string)Data[nameof(UserId)]!);
    private set => Data[nameof(UserId)] = value.Value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="IncorrectUserPasswordException"/> class.
  /// </summary>
  /// <param name="user">The user.</param>
  public UserHasNoPasswordException(UserAggregate user) : base(BuildMessage(user))
  {
    UserId = user.Id;
  }

  private static string BuildMessage(UserAggregate user) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(UserId), user.Id.Value)
    .Build();
}
