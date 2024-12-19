namespace Logitar.Identity.Core.Users;

/// <summary>
/// The exception raised when an user password check fails.
/// </summary>
public class IncorrectUserPasswordException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified password did not match the user.";

  /// <summary>
  /// Gets or sets the identifier of the user.
  /// </summary>
  public string UserId
  {
    get => (string)Data[nameof(UserId)]!;
    private set => Data[nameof(UserId)] = value;
  }
  /// <summary>
  /// Gets or sets the attempted password.
  /// </summary>
  public string AttemptedPassword
  {
    get => (string)Data[nameof(AttemptedPassword)]!;
    private set => Data[nameof(AttemptedPassword)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="IncorrectUserPasswordException"/> class.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <param name="attemptedPassword">The attempted password.</param>
  public IncorrectUserPasswordException(User user, string attemptedPassword)
    : base(BuildMessage(user, attemptedPassword))
  {
    AttemptedPassword = attemptedPassword;
    UserId = user.Id.Value;
  }

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <param name="attemptedPassword">The attempted password.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(User user, string attemptedPassword) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(UserId), user.Id)
    .AddData(nameof(AttemptedPassword), attemptedPassword)
    .Build();
}
