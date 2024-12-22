namespace Logitar.Identity.Core.Users;

/// <summary>
/// The exception raised when handling a user without a password.
/// </summary>
public class UserHasNoPasswordException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified user has no password.";

  /// <summary>
  /// Gets or sets the identifier of the user.
  /// </summary>
  public string UserId
  {
    get => (string)Data[nameof(UserId)]!;
    private set => Data[nameof(UserId)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserHasNoPasswordException"/> class.
  /// </summary>
  /// <param name="user">The user.</param>
  public UserHasNoPasswordException(User user) : base(BuildMessage(user))
  {
    UserId = user.Id.Value;
  }

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(User user) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(UserId), user.Id)
    .Build();
}
