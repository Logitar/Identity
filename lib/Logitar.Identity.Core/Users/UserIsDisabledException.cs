namespace Logitar.Identity.Core.Users;

/// <summary>
/// The exception raised when a disabled user is authenticated or signs-in.
/// </summary>
public class UserIsDisabledException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified user is disabled.";

  /// <summary>
  /// Gets the identifier of the disabled user.
  /// </summary>
  public string UserId
  {
    get => (string)Data[nameof(UserId)]!;
    private set => Data[nameof(UserId)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserIsDisabledException"/> class.
  /// </summary>
  /// <param name="user">The user that is disabled.</param>
  public UserIsDisabledException(User user) : base(BuildMessage(user))
  {
    UserId = user.Id.Value;
  }

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="user">The user that is disabled.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(User user) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(UserId), user.Id)
    .Build();
}
