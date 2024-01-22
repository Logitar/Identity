using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Passwords;

/// <summary>
/// The exception raised when a One-Time Password (OTP) validation fails.
/// </summary>
public class IncorrectOneTimePasswordPasswordException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public new const string ErrorMessage = "The specified password did not match the One-Time Password (OTP).";

  /// <summary>
  /// Gets or sets the identifier of the One-Time Password (OTP).
  /// </summary>
  public OneTimePasswordId OneTimePasswordId
  {
    get => new((string)Data[nameof(OneTimePasswordId)]!);
    private set => Data[nameof(OneTimePasswordId)] = value.Value;
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
  /// Initializes a new instance of the <see cref="IncorrectOneTimePasswordPasswordException"/> class.
  /// </summary>
  /// <param name="oneTimePassword">The One-Time Password (OTP).</param>
  /// <param name="attemptedPassword">The attempted password.</param>
  public IncorrectOneTimePasswordPasswordException(OneTimePasswordAggregate oneTimePassword, string attemptedPassword)
    : base(BuildMessage(oneTimePassword, attemptedPassword))
  {
    AttemptedPassword = attemptedPassword;
    OneTimePasswordId = oneTimePassword.Id;
  }

  private static string BuildMessage(OneTimePasswordAggregate oneTimePassword, string attemptedPassword) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(OneTimePasswordId), oneTimePassword.Id.Value)
    .AddData(nameof(AttemptedPassword), attemptedPassword)
    .Build();
}
