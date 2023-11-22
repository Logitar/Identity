using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// The exception raised when an user password check fails.
/// </summary>
public class IncorrectUserPasswordException : InvalidCredentialsException
{
  private const string ErrorMessage = "The specified password did not match the user.";

  /// <summary>
  /// Gets or sets the identifier of the user.
  /// </summary>
  public UserId UserId
  {
    get => new((string)Data[nameof(UserId)]!);
    private set => Data[nameof(UserId)] = value.Value;
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
  public IncorrectUserPasswordException(UserAggregate user, string attemptedPassword)
    : base(BuildMessage(user, attemptedPassword))
  {
    AttemptedPassword = attemptedPassword;
    UserId = user.Id;
  }

  private static string BuildMessage(UserAggregate user, string attemptedPassword) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(UserId), user.Id.Value)
    .AddData(nameof(AttemptedPassword), attemptedPassword)
    .Build();
}
